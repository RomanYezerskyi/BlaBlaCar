using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class BookedTrip
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public ICollection<BookedSeat> BookedSeats { get; set; }

    }
}
