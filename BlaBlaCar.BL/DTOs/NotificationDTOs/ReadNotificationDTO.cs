using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.NotificationDTOs
{
    public class ReadNotificationDTO
    {
        public Guid Id { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationDTO Notification { get; set; }
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
