﻿using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.Interfaces;
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
        public async Task<IActionResult> ReadUserNotification(IEnumerable<NotificationDTO> notification)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));
                var res = await _notificationService.ReadAllNotificationAsync(notification, User);
                if (res) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
