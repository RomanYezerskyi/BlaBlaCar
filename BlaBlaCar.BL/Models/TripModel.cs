using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Models
{
    public class TripModel
    {
        public int Id { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public int CountOfSeats { get; set; }
        public string UserId { get; set; }
        public ICollection<SeatModel> Seats { get; set; }
    }
}
