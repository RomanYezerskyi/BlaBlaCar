using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ODT.TripModels
{
    public class TripModel
    {
        public int Id { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public int CarId { get; set; }
        public CarModel Car { get; set; }
        public ICollection<TripUserModel> TripUsers { get; set; }
        public ICollection<AvailableSeatsModel> AvailableSeats { get; set; }
    }
}
