namespace BlaBlaCar.BL.Hubs
{
    public interface INotificationsHubClient
    {
        Task BroadcastNotification();
    }
}
