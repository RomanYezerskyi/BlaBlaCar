﻿using System;
using System.Collections.Generic;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.NotificationDTOs
{
    public class GetNotificationsDTO
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public UserDTO? User { get; set; }
        public NotificationDTOStatus NotificationStatus { get; set; }
        public NotificationStatusDTO ReadNotificationStatus { get; set; } = NotificationStatusDTO.NotRead;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
