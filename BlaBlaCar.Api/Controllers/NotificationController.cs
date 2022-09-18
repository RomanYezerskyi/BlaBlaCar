using BlaBlaCar.BL.DTOs.FeedbackDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Hubs.Interfaces;
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

        [HttpGet("unread")]
        public async Task<IActionResult> GetUserUnReadNotifications()
        {
            
            var res = await _notificationService.GetUserUnreadNotificationsAsync(User);
            if (res.Any()) return Ok(res);
            return NoContent();
           
        }
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] int take, [FromQuery] int skip)
        {

            var res = await _notificationService.GetUserNotificationsAsync(User, take, skip);
            if (res.Any()) return Ok(res);
            return NoContent();

        }
        [HttpPost]
        public async Task<IActionResult> ReadUserNotifications(IEnumerable<NotificationsDTO> notifications)
        {
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
            var res = await _notificationService.CreateNotificationAsync(notification, User);
            if (res) return Ok();
            return BadRequest();

        }
        [HttpPost("feedback")]
        public async Task<IActionResult> CreateFeedBack(CreateFeedbackDTO feedback)
        {
            await _notificationService.AddFeedBack(feedback, User);
            return NoContent();
        }
    }
}
