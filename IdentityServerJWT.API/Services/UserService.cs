using System.Data;
using System.Security.Claims;
using IdentityModel;
using IdentityServerJWT.API.Interfaces;
using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerJWT.API.Services
{
    public class UserService:IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }
        public async Task<List<UserWithRolesModel>> GetAdminsAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("blablacar.admin");
            List<UserWithRolesModel> admins = new List<UserWithRolesModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                admins.Add(new UserWithRolesModel(user){ Roles = new List<IdentityRole>() { new IdentityRole(roles.First()) } });
            }
            return admins;
        }

        public async Task<UserWithRolesModel> GetUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            UserWithRolesModel userWithRoles = new UserWithRolesModel(user)
            {
                Roles = new List<IdentityRole>() { new IdentityRole(roles.First()) }
            };
            return userWithRoles;
        }

        public async Task<IdentityResult> ChangeUserRole(UserChangeRoleModel roleModel)
        {
            var user = await _userManager.FindByIdAsync(roleModel.UserId);
            var allRoles = _roleManager.Roles.Where(x => x.Name != roleModel.RoleName);
            var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
            await _userManager.AddToRoleAsync(user, role.Name);
            var claims =await _userManager.GetClaimsAsync(user);
            await _userManager.ReplaceClaimAsync(user, claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Role),
                new Claim(JwtClaimTypes.Role, role.Name));
            return await _userManager.RemoveFromRoleAsync(user, allRoles.First().Name);

        }

        public async Task<IdentityResult> UpdateUser(UpdateUser userModel)
        {
            var user = await _userManager.FindByIdAsync(userModel.Id);

            if (user == null) throw new Exception("User not found");

            user.FirstName = userModel.FirstName;
            user.Email = userModel.Email;
            user.PhoneNumber = userModel.PhoneNumber;

            var claims = await _userManager.GetClaimsAsync(user);
            await _userManager.ReplaceClaimAsync(user, claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName),
                new Claim(ClaimTypes.GivenName, user.FirstName));
            await _userManager.ReplaceClaimAsync(user, claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name),
                new Claim(JwtClaimTypes.Name, user.Email));
            await _userManager.ReplaceClaimAsync(user, claims.FirstOrDefault(x => x.Type == ClaimTypes.Email),
                new Claim(ClaimTypes.Email, user.Email));
            await _userManager.ReplaceClaimAsync(user, claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PhoneNumber),
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));

            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> UpdateUserPassword(UpdateUserPassword newPasswordModel)
        {
            var user = await _userManager.FindByIdAsync(newPasswordModel.UserId);
            if (user == null) throw new Exception("User not found !");
            var result = await _userManager.ChangePasswordAsync(user, newPasswordModel.CurrentPassword, newPasswordModel.NewPassword);
            return result;
        }
    }
}
