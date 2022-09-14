using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs;

namespace BlaBlaCar.BL.DTOs.UserDTOs
{
    public class ChangeUserStatusDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public UserStatusDTO Status { get; set; }
    }
}
