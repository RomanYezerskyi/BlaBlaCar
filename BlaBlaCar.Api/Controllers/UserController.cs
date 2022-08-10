using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }
}
