using BlaBlaCar.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT.CarModels
{
    public class SeatModel
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public int CarId { get; set; }
        public CarModel Car { get; set; }
        public ICollection<TripUserModel> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }
    }
}
