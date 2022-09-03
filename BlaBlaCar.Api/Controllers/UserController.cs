using System.Net.Http.Headers;
using System.Security.Claims;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace BlaBlaCar.API.Controllers
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
        [HttpPost("license")]
        public async Task<IActionResult> AddDrivingLicense(IEnumerable<IFormFile> fileToUpload)
        {
            if (fileToUpload is null)
                return BadRequest();
            var res = await _userService.RequestForDrivingLicense(User, fileToUpload);
            if (res != null) return Ok(new {res} );
            return BadRequest("Fail");
          
        }
        [HttpGet]
        public async Task<IActionResult> GerUserInformation()
        {
            
            var res = await _userService.GetUserInformationAsync(User);
            if (res != null) return Ok(res);
            return BadRequest("Fail");
          
        }
        [HttpGet("users/{userData}")]
        public async Task<IActionResult> SearchUsersInformation(string userData)
        {

            var res = await _userService.SearchUsersAsync(userData);
            if (res != null) return Ok(res);
            return BadRequest("Fail");

        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userModel)
        {
           
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _userService.UpdateUserAsync(userModel, User);
            if (res)
                return Ok();
            return BadRequest();
            
        }
        [HttpPost("updateUserImg")]
        public async Task<IActionResult> UpdateUserImg(IFormFile userImg)
        {
           
            var res = await _userService.UpdateUserImgAsync(userImg, User);
            if (res)
                return Ok();
            return BadRequest();
           
        }
    }
}
