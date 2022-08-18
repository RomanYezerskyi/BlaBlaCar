using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT.BookTripModels
{
    public class AddNewBookTrip
    {
        public int RequestedSeats { get; set; }
        public Guid TripId { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<BookedSeatModel> BookedSeats { get; set; }
    }
}
