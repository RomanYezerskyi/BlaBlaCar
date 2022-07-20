using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Models
{
    public class BookedSeatModel
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public SeatModel Seat { get; set; }
        public int BookedTripId { get; set; }
        public BookedTripModel BookedTrip { get; set; }
    }
}
