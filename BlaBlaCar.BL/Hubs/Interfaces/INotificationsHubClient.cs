namespace BlaBlaCar.BL.Hubs.Interfaces
{
    public interface INotificationsHubClient
    {
        Task BroadcastNotification();
    }
}
