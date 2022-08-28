using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.NotificationDTOs
{
    public class CreateNotificationDTO
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public NotificationDTOStatus NotificationStatus { get; set; }
    }
}
