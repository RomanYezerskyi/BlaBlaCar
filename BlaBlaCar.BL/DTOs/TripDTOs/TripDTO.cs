using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using NetTopologySuite.Geometries;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class TripDTO:BaseDTO
    {
       
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public Guid CarId { get; set; }
        public CarDTO? Car { get; set; }
        public UserPermission UserPermission { get; set; }
        public ICollection<TripUserDTO>? TripUsers { get; set; }
        public ICollection<AvailableSeatDTO>? AvailableSeats { get; set; }
        public Point? StartLocation { get; set; }
        public Point? EndLocation { get; set; }
        public double StartLat => StartLocation != null ? StartLocation.X : default;

        public double StartLon => StartLocation != null ? StartLocation.Y : default;

        public double EndLat => EndLocation != null ? EndLocation.X : default;

        public double EndLon => EndLocation != null ? EndLocation.Y : default;
    }
}
