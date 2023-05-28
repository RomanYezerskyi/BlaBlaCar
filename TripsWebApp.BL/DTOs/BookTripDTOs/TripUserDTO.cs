using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.BookTripDTOs
{
    public class TripUserDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public Guid SeatId { get; set; }
        public SeatDTO? Seat { get; set; }
        public Guid TripId { get; set; }
        public TripDTO? Trip { get; set; }
    }
}
