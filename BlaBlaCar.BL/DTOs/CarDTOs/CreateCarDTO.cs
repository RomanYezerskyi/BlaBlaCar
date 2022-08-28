using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class CreateCarDTO
    {
        [Required]
        public string ModelName { get; set; }
        [Required]
        public string RegistNum { get; set; }
        [Required]
        public int CountOfSeats { get; set; }
        [Required]
        public IEnumerable<IFormFile> TechPassportFile { get; set; }
    }
}
