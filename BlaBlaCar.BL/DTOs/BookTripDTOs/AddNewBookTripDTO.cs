using System.ComponentModel.DataAnnotations;
using BlaBlaCar.BL.DTOs.BookTripDTOs;

namespace BlaBlaCar.BL.DTOs.BookTripModels
{
    public class AddNewBookTripDTO
    {
        [Required]
        public int RequestedSeats { get; set; }
        [Required]
        public Guid TripId { get; set; }
        public Guid? UserId { get; set; }
        [Required]
        public ICollection<BookedSeatDTO> BookedSeats { get; set; }
    }
}
