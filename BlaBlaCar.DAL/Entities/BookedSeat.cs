using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class BookedSeat
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int BookedTripId { get; set; }
        public BookedTrip BookedTrip { get; set; }
    }
}
