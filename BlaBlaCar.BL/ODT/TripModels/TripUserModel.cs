using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ODT.TripModels
{
    public class TripUserModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
