using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class TripUser
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public ApplicationUser User { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
