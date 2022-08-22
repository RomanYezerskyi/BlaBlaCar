using System.Net.Http.Headers;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
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
            try
            {
                if (fileToUpload is null)
                    return BadRequest();
                var res = await _userService.RequestForDrivingLicense(User, fileToUpload);
                if (res != null) return Ok(new {res} );
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GerUserInformation()
        {
            try
            {
                var res = await _userService.GetUserInformationAsync(User);
                if (res != null) return Ok(res);
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel userModel)
        {
            try
            {
                var res = await _userService.UpdateUserAsync(userModel, User);
                if (res)
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("updateUserImg")]
        public async Task<IActionResult> UpdateUserImg(IFormFile userImg)
        {
            try
            {
                var res = await _userService.UpdateUserImgAsync(userImg, User);
                if (res)
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
