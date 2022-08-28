using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class TripDTO
    {
        public Guid Id { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public Guid CarId { get; set; }
        public CarDTO? Car { get; set; }
        public ICollection<TripUserDTO>? TripUsers { get; set; }
        public ICollection<AvailableSeatDTO>? AvailableSeats { get; set; }
    }
}
