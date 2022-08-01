using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string RegistNum { get; set; }
        public CarType CarType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Trip> Trips { get; set; }
    }
}
