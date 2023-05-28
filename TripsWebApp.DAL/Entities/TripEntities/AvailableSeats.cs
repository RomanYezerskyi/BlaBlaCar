using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.DAL.Entities.CarEntities;

namespace BlaBlaCar.DAL.Entities.TripEntities
{
    public class AvailableSeats : BaseEntity
    {
        //public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; }
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; }
    }
}
