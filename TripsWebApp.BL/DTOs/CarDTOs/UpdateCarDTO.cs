using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class UpdateCarDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public int CountOfSeats { get; set; }
        [Required]
        public CarTypeDTO CarType { get; set; }
       // [Required]
        public IEnumerable<IFormFile>? TechnicalPassportFile { get; set; }
        public IEnumerable<CarDocumentDTO>? DeletedDocuments { get; set; }
    }
}
