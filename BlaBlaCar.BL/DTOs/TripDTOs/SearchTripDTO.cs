using System.ComponentModel.DataAnnotations;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class SearchTripDTO
    {
        [Required]
        public string StartPlace { get; set; }
        [Required]
        public string EndPlace { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public int CountOfSeats { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = int.MaxValue;
    }
}
