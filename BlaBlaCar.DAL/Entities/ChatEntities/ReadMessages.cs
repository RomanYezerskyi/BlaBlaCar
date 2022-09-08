using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities.ChatEntities
{
    public class ReadMessages:BaseEntity
    {
        public Guid MessageId { get; set; }
        public Message Message { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
