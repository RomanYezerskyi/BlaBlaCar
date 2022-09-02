using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ViewModels.AdminViewModels;
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

        [HttpGet("requests/{status}/{take}/{skip}")]
        public async Task<IActionResult> GetRequests(int take, int skip, UserDTOStatus status)
        {
            
            var res = await _adminService.GetRequestsAsync(take, skip, status);
            if (res.Users.Any()) return Ok(res);
            return NoContent();
          
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRequest(Guid id)
        {
           
            var res = await _adminService.GetUserRequestAsync(id);
            if (res != null) return Ok(res);
            return BadRequest(res);
           
        }
        [HttpPost("user/status")]
        public async Task<IActionResult> ChangeUserRequest([FromBody]ChangeUserStatusDTO status)
        {
            
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));

            var res = await _adminService.ChangeUserStatusAsync(status, User);
            if (res != null) return Ok("User status changed");
            return Ok(new JsonResult("User status changed!"));
           
        }
        [HttpPost("car/status")]
        public async Task<IActionResult> ChangeCarRequest([FromBody] ChangeCarStatusDTO status)
        {
            
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));

            var res = await _adminService.ChangeCarStatusAsync(status, User);
            if (res != null) return Ok(new{res="User status changed"});
            return BadRequest(res);
            
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatic() {
            var res = await _adminService.GetStatisticsDataAsync();
            return Ok(res);
        }

        [HttpGet("top-list/{take}/{skip}/{orderBy}")]
        public async Task<IActionResult> GetUsersTopList(int take, int skip, UsersListOrderByType orderBy)
         {
            var res = await _adminService.GetTopUsersListAsync(take,skip, orderBy);
            return Ok(res);
        }
    }
}
