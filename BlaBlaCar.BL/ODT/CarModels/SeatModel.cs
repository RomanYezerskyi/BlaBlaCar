using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.DAL.Entities.TripEntities;

namespace BlaBlaCar.BL.ODT.CarModels
{
    public class SeatModel
    {
        public Guid Id { get; set; }
        public int Num { get; set; }
        public Guid CarId { get; set; }
        public CarModel Car { get; set; }
        public ICollection<TripUserModel> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }
    }
}
