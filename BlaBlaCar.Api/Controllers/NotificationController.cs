using BlaBlaCar.BL.DTOs.FeedbackDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Hubs.Interfaces;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.AdminOrUser)]
    public class NotificationController : CustomBaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUserUnReadNotifications()
        {
            
            var res = await _notificationService.GetUserUnreadNotificationsAsync(UserId);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] int take, [FromQuery] int skip)
        {

            var res = await _notificationService.GetUserNotificationsAsync(take, skip, UserId);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> ReadUserNotifications(IEnumerable<NotificationsDTO> notifications)
        {
            var res = await _notificationService.ReadAllNotificationAsync(notifications, UserId);
            return NoContent();
        }
        [HttpGet("global")]
        public async Task<IActionResult> GetGlobalNotifications([FromQuery] int take,[FromQuery] int skip)
        {

            var res = await _notificationService.GetGlobalNotificationsAsync(take,skip); 
            return Ok(res);
        }
        [HttpGet("last-notifications")]
        public async Task<IActionResult> GetUsersNotifications([FromQuery]int take,[FromQuery] int skip)
        {

            var res = await _notificationService.GetUsersNotificationsAsync(take, skip);
            return Ok(res);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification(CreateNotificationDTO notification)
        {
            var res = await _notificationService.CreateNotificationAsync(notification, UserId);
            return NoContent();
        }
        [HttpPost("feedback")]
        public async Task<IActionResult> CreateFeedBack(CreateFeedbackDTO feedback)
        {
            await _notificationService.AddFeedBack(feedback, UserId);
            return NoContent();
        }
        [HttpGet("user-feedbacks")]
        public async Task<IActionResult> CreateFeedBack([FromQuery] int take, [FromQuery] int skip)
        {
            var result = await _notificationService.GetUserFeedBacks(UserId, take, skip);
            return Ok(result);
        }
    }
}
