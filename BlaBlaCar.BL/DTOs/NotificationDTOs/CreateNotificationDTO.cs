using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.NotificationDTOs
{
    public class CreateNotificationDTO
    {
        [Required]
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public NotificationStatusDTO NotificationStatus { get; set; }
    }
}
