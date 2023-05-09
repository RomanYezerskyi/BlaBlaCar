using BlaBlaCar.BL.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BlaBlaCar.BL.Hubs;

public class MapHub: Hub<IMapHubClient>
{
    public async Task JoinToMapHub(string tripId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, tripId);
    }
}