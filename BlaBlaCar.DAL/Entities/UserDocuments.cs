using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class UserDocuments
    {
        public Guid Id { get; set; }
        public string DrivingLicense { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
