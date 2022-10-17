using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;

using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class ChangeCarStatus
    {
        [Required]
        public Guid CarId { get; set; }
        public CarStatus Status { get; set; }
    }
}
