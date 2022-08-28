using System;
using System.Collections.Generic;

namespace BlaBlaCar.BL.DTOs.NotificationDTOs
{
    public class GetNotificationDTO
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public NotificationDTOStatus NotificationStatus { get; set; }
        public NotificationStatusDTO ReadNotificationStatus { get; set; } = NotificationStatusDTO.NotRead;
        public DateTime CreatedAt { get; set; }
    }
}
