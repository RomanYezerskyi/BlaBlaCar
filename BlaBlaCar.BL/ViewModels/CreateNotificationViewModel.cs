using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.NotificationModels;

namespace BlaBlaCar.BL.ViewModels
{
    public class CreateNotificationViewModel
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public NotificationModelStatus NotificationStatus { get; set; }
    }
}
