using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class Trip
    {
        public Guid Id { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }
        public ICollection<TripUser> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }
    }
}
