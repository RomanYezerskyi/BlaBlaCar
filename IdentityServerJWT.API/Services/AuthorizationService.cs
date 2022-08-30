using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using IdentityServerJWT.API.Interfaces;
using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServerJWT.API.Services
{
    public class AuthorizationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthorizationService(IOptionsSnapshot<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedResponse> Login(LoginModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user is null)
            {
                throw new Exception("Invalid client request");
            }

            var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, false, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                //var claims = new List<Claim>
                //{
                //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //    new Claim(JwtClaimTypes.Id, user.Id.ToString())
                //};


                var claims = await _userManager.GetClaimsAsync(user);
                //if (roles != null)
                //{
                //    var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
                //    claims.ToList().AddRange(roleClaims);
                //}

                var accessToken = _tokenService.GenerateAccessTokenTokenAsync(claims);
                var refreshToken = _tokenService.GenerateRefreshTokenAsync();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTimeOffset.Now.AddMinutes(_jwtSettings.RefreshInMinutes);

                await _userManager.UpdateAsync(user);
                

                //var tokenString = GenerateJwt(user, roles);
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
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }




            //var newRole = new IdentityRole
            //{
            //    Name = "blablacar.user"
            //};
            //var roleResult = await _roleManager.CreateAsync(newRole);
            var result1 = await _userManager.AddToRoleAsync(user, "blablacar.user");

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

    }
}
