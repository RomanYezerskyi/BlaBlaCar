using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities.NotificationEntities
{
    public class Notification : BaseEntity
    {
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public ICollection<ReadNotification> ReadNotifications { get; set; }
    }
}
