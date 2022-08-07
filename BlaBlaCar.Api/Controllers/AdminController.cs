using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "blablacar.admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("requests/{status}")]
        public async Task<IActionResult> GetPendingRequests(ModelStatus status)
        {
            try
            {
                var res = await _adminService.GetRequestsAsync(status);
                if (res.Any()) return Ok(res);
                return BadRequest(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRequests(string id)
        {
            try
            {
                var res = await _adminService.GetUserRequestsAsync(id);
                if (res != null) return Ok(res);
                return BadRequest(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost("user/status")]
        public async Task<IActionResult> ChangeTaskUserRequest([FromBody]ChangeUserStatus status)
        {
            try
            {
                var res = await _adminService.ChangeUserStatusAsync(status);
                if (res != null) return Ok("User status changed");
                return BadRequest(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost("car/status")]
        public async Task<IActionResult> ChangeCarRequest([FromBody] ChangeCarStatus status)
        {
            try
            {
                var res = await _adminService.ChangeCarStatusAsync(status);
                if (res != null) return Ok("User status changed");
                return BadRequest(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
