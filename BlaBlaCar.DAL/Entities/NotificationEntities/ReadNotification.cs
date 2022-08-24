using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities.NotificationEntities
{
    public class ReadNotification: BaseEntity
    {
        public Guid NotificationId { get; set; }
        public Notification Notification { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
