using System.Security.Claims;
using IdentityModel;
using IdentityServerJWT.API.Interfaces;
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
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("roles")]
        [Authorize(Roles = "blablacar.admin")]
        public async Task<IActionResult> Roles()
        {
            try
            {
                var roles = await _userService.GetRoles();
                return Ok(roles);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpGet("{email}")]
        [Authorize(Roles = "blablacar.admin")]
        public async Task<IActionResult> GetUser(string email)
        {
            try
            {
                var user = await _userService.GetUser(email);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
           
        }
        [HttpPost]
        [Authorize(Roles = "blablacar.admin")]
        public async Task<IActionResult>ChangeRole(UserChangeRoleModel roleModel)
        {

            try
            {
                if (!ModelState.IsValid) throw new Exception("All data is required !");
                var user = await _userService.ChangeUserRole(roleModel);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
          
        }
        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUser userModel)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("All data is required !");

                var result = await _userService.UpdateUser(userModel);
                if (result.Succeeded)
                    return Ok(result);
                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
           
        }
        [HttpPost("update-password")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPassword newPasswordModel)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("All data is required !");

                var result = await _userService.UpdateUserPassword(newPasswordModel);
                if (result.Succeeded)
                    return Ok(result);
                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
           
        }

    }
}
