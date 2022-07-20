using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Models
{
    public class BookedTripModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
        ICollection<BookedSeatModel> BookedSeats { get; set; }

    }
}
