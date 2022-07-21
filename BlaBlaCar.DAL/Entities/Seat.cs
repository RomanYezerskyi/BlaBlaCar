using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
