using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
                //var data = System.IO.File.ReadAllBytes("DriverDocuments\\Images\\3f7302cb-0cd7-4305-a550-65a46c029ae9.jpg");


                //var d =new List<byte[]>();
                //d.Add(data);
                //d.Add(data);
                //FileInfo aInfo = new FileInfo(id);

                //return Ok(File(data, MediaTypeNames.Image.Jpeg, "3f7302cb-0cd7-4305-a550-65a46c029ae9.jpg"));
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
                //var res = await _adminService.ChangeUserStatusAsync(status);
                //if (res != null) return Ok("User status changed
                return Ok(new JsonResult("User status changed!"));
               //return BadRequest("bad");
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
            //    var res = await _adminService.ChangeCarStatusAsync(status);
            //    if (res != null) return Ok("User status changed");
                //return BadRequest(res);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
