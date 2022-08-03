using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ODT.CarModels;

namespace BlaBlaCar.DAL.Entities
{
    public class AvailableSeatsModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
        public int SeatId { get; set; }
        public SeatModel Seat { get; set; }
        public AvailableSeatsType AvailableSeatsType { get; set; }
    }
}
