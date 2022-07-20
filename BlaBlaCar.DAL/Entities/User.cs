using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class User: IdentityUser
    {
        public ICollection<UserTrip> UserTrips { get; set; }
        public ICollection<BookedTrip> BookedTrips { get; set; }
    }
}
