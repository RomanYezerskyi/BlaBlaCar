using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.NotificationModels;

namespace BlaBlaCar.BL.ViewModels
{
    public class GetNotificationViewModel
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public NotificationModelStatus NotificationStatus { get; set; }
        public ReadNotificationStatus ReadNotificationStatus { get; set; } = ReadNotificationStatus.NotRead;
        public DateTime CreatedAt { get; set; }
    }
}
