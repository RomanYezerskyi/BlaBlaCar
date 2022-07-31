using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class Trip
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public ApplicationUser User { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public ICollection<BookedTrip> BookedTrips { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
