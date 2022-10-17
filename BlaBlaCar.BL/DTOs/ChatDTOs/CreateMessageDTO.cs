using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    public class CreateMessageDTO
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid ChatId { get; set; }
        [Required]
        public UserDTO User { get; set; }
    }
}
