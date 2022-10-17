using System.ComponentModel.DataAnnotations;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class CreateTripDTO
    {

        [Required]
        public double StartLat { get; set; }
        [Required]
        public double StartLon { get; set; }
        [Required]
        public double EndLat { get; set; }
        [Required]
        public double EndLon { get; set; }
        [Required]
        public DateTimeOffset StartTime { get; set; }
        [Required]
        public DateTimeOffset EndTime { get; set; }
        [Required]
        public int PricePerSeat { get; set; }
        public string? Description { get; set; }
        [Required]
        public int CountOfSeats { get; set; }
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public ICollection<NewAvailableSeatDTO> AvailableSeats { get; set; }

    }
}
