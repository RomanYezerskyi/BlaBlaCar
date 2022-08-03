using System.Security.Claims;
using IdentityModel;
using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "blablacar.admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            UserWithRolesModel userWithRoles = new UserWithRolesModel(user);
            userWithRoles.Roles = new List<IdentityRole>() { new IdentityRole(roles.First()) };
            return Ok(userWithRoles);
        }
        [HttpPost]
        public async Task<IActionResult>ChangeRole(UserChangeRoleModel roleModel)
        {

            var user = await _userManager.FindByIdAsync(roleModel.UserId);
            var allRoles = _roleManager.Roles.ToList().First();
            var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
            await _userManager.AddToRoleAsync(user, role.Name);
            await _userManager.RemoveFromRoleAsync(user, allRoles.Name);
            return Ok();
        }
    }
}
