using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.AdminDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.AdminRole)]
    public class AdminController : CustomBaseController
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests([FromQuery] int take,[FromQuery] int skip,[FromQuery] UserStatusDTO status)
        {
            var res = await _adminService.GetRequestsAsync(take, skip, status);
            if (res.Users.Any()) return Ok(res);
            return NoContent();
          
        }
        
        [HttpPatch("user/status")]
        public async Task<IActionResult> ChangeUserRequest([FromBody]ChangeUserStatusDTO status)
        {
            var res = await _adminService.ChangeUserStatusAsync(status, UserId);
            return NoContent();

        }
        [HttpPatch("car/status")]
        public async Task<IActionResult> ChangeCarRequest([FromBody] ChangeCarStatus status)
        {
            var res = await _adminService.ChangeCarStatusAsync(status, UserId);
            return NoContent();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery]string searchDate)
        {
            DateTimeOffset.TryParse(searchDate, out var date);
            var res = await _adminService.GetStatisticsDataAsync(date);
            return Ok(res);
        }
        [HttpGet("short-statistics")]
        public async Task<IActionResult> GetShortStatistics()
        {
            
            var res = await _adminService.GetShortStatisticsDataAsync();
            return Ok(res);
        }

        [HttpGet("top-list")]
        public async Task<IActionResult> GetUsersTopList([FromQuery] int take,[FromQuery] int skip,[FromQuery] UsersListOrderByType orderBy)
         {
            var res = await _adminService.GetTopUsersListAsync(take,skip, orderBy);
            return Ok(res);
        }
    }
}
