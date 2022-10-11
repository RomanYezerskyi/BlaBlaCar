using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmailService.Models;
using EmailService.Services;
using IdentityModel;
using IdentityServerJWT.API.Interfaces;
using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IdentityServerJWT.API.Services
{
    public class AuthorizationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public AuthorizationService(IOptionsSnapshot<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, 
            IEmailSender emailSender)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        public async Task<AuthenticatedResponse> Login(LoginModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            
            if (user is null)
            {
                throw new Exception("Invalid client request");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw  new Exception( "Email is not confirmed! Please check your email!" );

            var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, false, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = await _userManager.GetClaimsAsync(user);
                var accessToken = _tokenService.GenerateAccessTokenTokenAsync(claims);
                var refreshToken = _tokenService.GenerateRefreshTokenAsync();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTimeOffset.Now.AddMinutes(_jwtSettings.RefreshInMinutes);

                await _userManager.UpdateAsync(user);
                
                return new AuthenticatedResponse { Token = accessToken, RefreshToken = refreshToken };
            }

            throw new Exception("Incorrect login or password");
        }
        public async Task<IdentityResult> Register(RegisterModel userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.Email,
                PhoneNumber = userModel.PhoneNum,
                FirstName = userModel.FirstName,
                Email = userModel.Email,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(userModel.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback);
            await _emailSender.SendEmailAsync(message);

            var result1 = await _userManager.AddToRoleAsync(user, Constants.UserRole);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(JwtClaimTypes.Role, r));
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                new Claim(ClaimTypes.GivenName, user.FirstName)
            };
            claims.AddRange(roleClaims);
            result = await _userManager.AddClaimsAsync(user, claims);
            return result;
        }

        public async Task GenerateForgotPasswordTokenAsync(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new Exception("Invalid Request");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", model.Email }
            };
            var callback = QueryHelpers.AddQueryString(model.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback);
            await _emailSender.SendEmailAsync(message);
        }
        public async Task ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new Exception("Invalid Request");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                throw new Exception(string.Join(", ", errors));
            }
            
        }
        public async Task EmailConfirmationAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid Email Confirmation Request");
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                throw new Exception("Invalid Email Confirmation Request");
        }

    }
}
