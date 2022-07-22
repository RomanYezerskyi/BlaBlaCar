using IdentityServer.API.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityServerInteractionService _interactionService;
        public AuthController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _interactionService = interactionService;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel registerViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        throw new Exception("Invalid data");
        //    }

        //    var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
        //    if (user == null)
        //    {
        //        throw new Exception("User not found");
        //    }

        //    var res = await _signInManager
        //        .PasswordSignInAsync(user, registerViewModel.Password, false,false);
        //    //if (res)
        //    //{

        //    //}
        //    return new JsonResult("a");
        //}
    }
}
