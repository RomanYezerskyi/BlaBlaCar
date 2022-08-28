using System.ComponentModel.DataAnnotations;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class CreateTripDTO
    {
        [Required]
        public string StartPlace { get; set; }
        [Required]
        public string EndPlace { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public int PricePerSeat { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CountOfSeats { get; set; }
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public ICollection<NewAvailableSeatDTO> AvailableSeats { get; set; }

    }
}
