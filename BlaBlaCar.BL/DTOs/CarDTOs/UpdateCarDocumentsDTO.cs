using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class UpdateCarDocumentsDTO
    {
        [Required]
        public Guid Id { get; set; }
        public IEnumerable<IFormFile>? TechPassportFile { get; set; }
        public IEnumerable<string>? DeletedDocuments { get; set; }
    }
}
