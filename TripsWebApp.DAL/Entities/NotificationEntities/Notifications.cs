using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities.NotificationEntities
{
    public class Notifications : BaseEntity
    {
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public Guid? FeedBackOnUser { get; set; }
        public ICollection<ReadNotifications> ReadNotifications { get; set; }
    }
}
