using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int Num { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }
        public ICollection<TripUser> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }
    }
}
