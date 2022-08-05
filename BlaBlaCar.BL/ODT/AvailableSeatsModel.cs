using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT
{
    public class AvailableSeatsModel
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public TripModel Trip { get; set; }
        public Guid SeatId { get; set; }
        public SeatModel Seat { get; set; }
        public AvailableSeatsType AvailableSeatsType { get; set; }
    }
}
