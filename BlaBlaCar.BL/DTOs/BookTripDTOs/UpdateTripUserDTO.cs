using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.BookTripDTOs
{
    public class UpdateTripUserDTO
    {
        [Required]
        public Guid SeatId { get; set; }
        [Required]
        public Guid TripId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
