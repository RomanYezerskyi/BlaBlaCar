namespace BlaBlaCar.BL.ODT.NotificationModels
{
    public class ReadNotificationModel
    {
        public Guid Id { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationModel Notification { get; set; }
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
    }
}
