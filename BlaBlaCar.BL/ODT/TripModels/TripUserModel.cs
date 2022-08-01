using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ODT.TripModels
{
    public class TripUserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
