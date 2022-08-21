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
    [Authorize]
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
        [Authorize(Roles = "blablacar.admin")]
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
        [Authorize(Roles = "blablacar.admin")]
        public async Task<IActionResult>ChangeRole(UserChangeRoleModel roleModel)
        {

            var user = await _userManager.FindByIdAsync(roleModel.UserId);
            var allRoles = _roleManager.Roles.Where(x=>x.Name != roleModel.RoleName);
            var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
            await _userManager.AddToRoleAsync(user, role.Name);
            await _userManager.RemoveFromRoleAsync(user, allRoles.First().Name);
            return Ok();
        }
        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUser userModel)
        {
            if (!ModelState.IsValid) throw new Exception("All data is required !");
            var user = await _userManager.FindByIdAsync(userModel.Id);

            if (user == null) throw new Exception("User not found");

            if (user.FirstName != userModel.FirstName)
                user.FirstName = userModel.FirstName;
            
            if (user.Email != userModel.Email) 
                user.Email = userModel.Email;

            if(user.PhoneNumber != userModel.PhoneNumber)
                user.PhoneNumber = userModel.PhoneNumber;
            
            var result =await _userManager.UpdateAsync(user);
            if(result.Succeeded)
                return Ok(result);
            return BadRequest(result.Errors);
        }

    }
}
