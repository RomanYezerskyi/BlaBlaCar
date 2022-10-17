using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class CarDTO:BaseDTO
    {
        public string ModelName { get; set; }
        public string RegistrationNumber { get; set; }
        public CarTypeDTO CarType { get; set; }
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
        public ICollection<CarDocumentDTO> CarDocuments { get; set; }
        public CarStatus CarStatus { get; set; }
        public ICollection<SeatDTO> Seats { get; set; }
        public ICollection<TripDTO> Trips { get; set; }
    }
}
