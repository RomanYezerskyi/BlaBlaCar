using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub, INotificationsHubClient> _hubContext;
        public NotificationController(INotificationService notificationService, IHubContext<NotificationHub, INotificationsHubClient> hubContext)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            
            var res = await _notificationService.GetUserNotificationsAsync(User);
            if (res.Any()) return Ok(res);
            return NoContent();
           
        }
        [HttpPost]
        public async Task<IActionResult> ReadUserNotifications(IEnumerable<NotificationsDTO> notifications)
        {
           
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _notificationService.ReadAllNotificationAsync(notifications, User);
            if (res) return Ok();
            return BadRequest();
            
        }
        [HttpGet("global/{take}/{skip}/")]
        public async Task<IActionResult> GetGlobalNotifications(int take, int skip)
        {

            var res = await _notificationService.GetGlobalNotificationsAsync(take,skip);
            if (res.Any()) return Ok(res);
            return NoContent();

        }
        [HttpGet("users/{take}/{skip}/")]
        public async Task<IActionResult> GetUsersNotifications(int take, int skip)
        {

            var res = await _notificationService.GetUsersNotificationsAsync(take, skip);
            if (res.Any()) return Ok(res);
            return NoContent();

        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification(CreateNotificationDTO notification)
        {

            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _notificationService.CreateNotificationAsync(notification, User);
            //await _hubContext.Clients.All.BroadcastMessage();
            if (res) return Ok();
            return BadRequest();

        }
    }
}
