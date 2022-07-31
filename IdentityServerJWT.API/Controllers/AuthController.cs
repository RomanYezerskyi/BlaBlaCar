﻿using System.IdentityModel.Tokens.Jwt;
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
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthorizationService _authorizationService;
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, AuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizationService = authorizationService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel userModel)
        {
            if (userModel is null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                var res = await _authorizationService.Login(userModel);
                //var a = User.FindFirst(JwtClaimTypes.Name).Value;
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
            //return Redirect("~/");
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
