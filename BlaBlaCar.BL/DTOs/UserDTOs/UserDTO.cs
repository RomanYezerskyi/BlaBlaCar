using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.DTOs.UserDTOs
{
    public class UserDTO:BaseDTO
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? UserImg { get; set; }
        public UserStatusDTO? UserStatus { get; set; }
        public ICollection<UserDocumentDTO>? UserDocuments { get; set; }
        public ICollection<CarDTO>? Cars { get; set; }
        public ICollection<TripDTO>? Trips { get; set; }
        public ICollection<TripUserDTO>? TripUsers { get; set; }
    }
}
