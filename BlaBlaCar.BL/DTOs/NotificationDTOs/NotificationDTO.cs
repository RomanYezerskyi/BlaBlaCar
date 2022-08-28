

using System.ComponentModel.DataAnnotations;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.NotificationDTOs
{
    public class NotificationDTO
    {
        [Required]
        public Guid Id { get; set; }
        public string? Text { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public NotificationDTOStatus? NotificationStatus { get; set; }
        public ICollection<ReadNotificationDTO>? ReadNotifications { get; set; }
    }
}
