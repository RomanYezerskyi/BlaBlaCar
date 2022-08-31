using System.ComponentModel.DataAnnotations;
using BlaBlaCar.BL.Services.TripServices;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class SearchTripDTO
    {
        [Required]
        public string StartPlace { get; set; }
        [Required]
        public string EndPlace { get; set; }
        [Required]
        public DateTimeOffset StartTime { get; set; }
        [Required]
        public int CountOfSeats { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = int.MaxValue;
        public TripOrderBy OrderBy { get; set; } = TripOrderBy.EarliestDepartureTime;
    }
}
