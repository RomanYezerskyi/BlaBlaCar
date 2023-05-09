using BlaBlaCar.BL.DTOs.MapDTOs;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Hubs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MapsController : ControllerBase
    {
        private readonly IHubContext<MapHub, IMapHubClient> _hubContext;

        public MapsController(IHubContext<MapHub, IMapHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPut("user-location")]
        public async Task<IActionResult> ReadChatMessages([FromBody] UserLocation location)
        {
            await _hubContext.Clients.Groups(location.TripId.ToString()).BroadcastLocation(location);
            return NoContent();
        }
    }
}
