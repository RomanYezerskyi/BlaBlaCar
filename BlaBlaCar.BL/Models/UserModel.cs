using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BlaBlaCar.BL.Models
{
    public class UserModel: IdentityUser
    {
        public ICollection<UserTripModel> UserTrips { get; set; }
        public ICollection<BookedTripModel> BookedTrips { get; set; }
    }
}
