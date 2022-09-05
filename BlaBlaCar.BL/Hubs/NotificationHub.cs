 using Microsoft.AspNetCore.SignalR;

namespace BlaBlaCar.BL.Hubs
{
    public class NotificationHub:Hub<INotificationsHubClient>
    {
        public async Task<string> GetConnectionId(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return Context.ConnectionId;
        }
    }
}
