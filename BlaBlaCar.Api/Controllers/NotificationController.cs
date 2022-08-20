using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.NotificationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotification()
        {
            try
            {
                var res = await _notificationService.GetUserNotificationsAsync(User);
                if (res.Any()) return Ok(res);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReadUserNotification(IEnumerable<NotificationModel> notification)
        {
            try
            {
                //var res = await _notificationService.ReadAllNotificationAsync(notification, User);
                //if (res) return Ok();
                //return BadRequest();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
