using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class FeedBack:BaseEntity
    {
        public string Text { get; set; }
        public byte Rate { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
