using BlaBlaCar.BL.DTOs.MapDTOs;

namespace BlaBlaCar.BL.Hubs.Interfaces;

public interface IMapHubClient
{
    Task BroadcastLocation(UserLocation location);
}