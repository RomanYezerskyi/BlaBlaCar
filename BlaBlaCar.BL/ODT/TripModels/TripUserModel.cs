using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ODT.TripModels
{
    public class TripUserModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserModel? User { get; set; }
        public Guid SeatId { get; set; }
        public SeatModel? Seat { get; set; }
        public Guid TripId { get; set; }
        public TripModel? Trip { get; set; }
    }
}
