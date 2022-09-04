using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.AdminDTOs
{
    public class UsersStatisticsDTO
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? UserImg { get; set; }
        public UserDTOStatus? UserStatus { get; set; }
        public ICollection<UserDocumentDTO>? UserDocuments { get; set; }
        public ICollection<CarDTO>? Cars { get; set; }
        public ICollection<TripDTO>? Trips { get; set; }
        public ICollection<TripUserDTO>? TripUsers { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
