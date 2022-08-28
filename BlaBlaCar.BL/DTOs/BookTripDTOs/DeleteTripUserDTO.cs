using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.TripDTOs;

namespace BlaBlaCar.BL.DTOs.BookTripDTOs
{
    public class DeleteTripUserDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string StartPlace { get; set; }
        [Required]
        public string EndPlace { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public ICollection<TripUserDTO>? TripUsers { get; set; }
    }
}
