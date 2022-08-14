using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.CarModels;

namespace BlaBlaCar.BL.ViewModels
{
    public class TripUserViewModel
    {
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public Guid TripId { get; set; }
        public string UserId { get; set; }
    }
}
