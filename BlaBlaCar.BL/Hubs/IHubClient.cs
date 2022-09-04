namespace BlaBlaCar.BL.Hubs
{
    public interface IHubClient
    {
        Task BroadcastMessage();
    }
}
