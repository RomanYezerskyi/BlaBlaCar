using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class Car: BaseEntity
    {
        public string ModelName { get; set; }
        public string RegistNum { get; set; }
        public CarType CarType { get; set; }
        public Guid UserId { get; set; }
        public Status CarStatus { get; set; }
        public ICollection<CarDocuments> CarDocuments { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Trip> Trips { get; set; }
    }
}
