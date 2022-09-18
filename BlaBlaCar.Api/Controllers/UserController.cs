using System.Net.Http.Headers;
using System.Security.Claims;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services.Admin;
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
        [HttpGet("user")]
        public async Task<IActionResult> AddUserToApi()
        {

            var res = await _userService.AddUserAsync(User);
            if (res != null) return Ok(new { res });
            return BadRequest("Fail");

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {

            var res = await _userService.GetUserByIdAsync(id);
            if (res != null) return Ok(res);
            return BadRequest(res);

        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userModel)
        {
            
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
        //[HttpGet("statistics")]
        //public async Task<IActionResult> GerUserStatistics()
        //{

        //    var res = await _userService.GetUserStatisticsAsync(User);
        //    if (res != null) return Ok(res);
        //    return BadRequest("Fail");

        //}

    }
}
