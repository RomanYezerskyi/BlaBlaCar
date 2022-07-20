using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Models
{
    public class SeatModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
    }
}
