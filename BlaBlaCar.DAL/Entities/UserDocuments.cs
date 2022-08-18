using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class UserDocuments:BaseEntity
    {
        //public Guid Id { get; set; }
        public string DrivingLicense { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
