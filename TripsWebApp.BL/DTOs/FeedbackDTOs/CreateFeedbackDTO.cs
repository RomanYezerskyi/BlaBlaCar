using BlaBlaCar.DAL.Entities.NotificationEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.NotificationDTOs;

namespace BlaBlaCar.BL.DTOs.FeedbackDTOs
{
    public class CreateFeedbackDTO
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public byte Rate { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
