using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.DAL.Entities.TripEntities;

namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public Guid CarId { get; set; }
        public CarDTO? Car { get; set; }
        public ICollection<TripUserDTO>? TripUsers { get; set; }
        public ICollection<AvailableSeats>? AvailableSeats { get; set; }
    }
}
