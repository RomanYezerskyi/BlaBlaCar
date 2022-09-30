using System.Net.Http.Headers;
using System.Security.Claims;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services;
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
    [Authorize(Roles = Constants.AdminOrUser)]

    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GerUserInformation()
        {

            var res = await _userService.GetUserInformationAsync(UserId);
            return Ok(res);
        }
        [HttpGet("add-user")]
        public async Task<IActionResult> AddUserToApi()
        {

            var res = await _userService.AddUserAsync(GetUserInformation());
            return Ok();
        }
        [HttpGet("documents")]
        public async Task<IActionResult> GetUserDocuments()
        {

            var res = await _userService.GetUserDocumentsAsync(UserId);
            return Ok(res);
        }
        [HttpPost("license")]
        public async Task<IActionResult> AddDrivingLicense([FromForm] UpdateUserDocuments documents)
        {
            var res = await _userService.RequestForDrivingLicense(documents, UserId);
           return Ok();
        }
       
        [Authorize(Roles = Constants.AdminRole)]
        [HttpGet("users/{userNameOrEmail}")]
        public async Task<IActionResult> SearchUsersInformation(string userNameOrEmail)
        {

            var res = await _userService.SearchUsersAsync(userNameOrEmail);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {

            var res = await _userService.GetUserByIdAsync(id);
            return Ok(res);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userModel)
        {
            
            var res = await _userService.UpdateUserAsync(userModel, UserId);
            return NoContent();
        }
        [HttpPut("user-profile-image")]
        public async Task<IActionResult> UpdateUserImg(IFormFile userProfileImage)
        {
           
            var link = await _userService.UpdateUserImgAsync(userProfileImage, UserId);
            return Ok(link);
        }
    }
}
