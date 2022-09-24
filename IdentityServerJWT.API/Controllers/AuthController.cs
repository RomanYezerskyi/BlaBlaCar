using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using IdentityServerJWT.API.Models;
using IdentityServerJWT.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServerJWT.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthorizationService _authorizationService;
        public AuthController(
            SignInManager<ApplicationUser> signInManager, AuthorizationService authorizationService)
        {
            _signInManager = signInManager;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
      
        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginModel userModel)
        {
            if (userModel is null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                var res = await _authorizationService.Login(userModel);
                return RedirectToRoute("http://localhost:4200/profile", res);
                return Ok(res);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel userModel)
        {
            if (userModel is null)
            {
                return BadRequest("Invalid client request");
            }
            try
            {
                var result = await _authorizationService.Register(userModel);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
