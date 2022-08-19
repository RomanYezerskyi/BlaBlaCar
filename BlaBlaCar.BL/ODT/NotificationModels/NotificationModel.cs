

namespace BlaBlaCar.BL.ODT.NotificationModels
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid? UserId { get; set; }
        public UserModel? User { get; set; }
        public NotificationModelStatus NotificationStatus { get; set; }
        public ICollection<ReadNotificationModel> ReadNotifications { get; set; }
    }
}
