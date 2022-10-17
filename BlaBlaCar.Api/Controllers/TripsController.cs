using System.Security.Claims;
using BlaBlaCar.API;
using BlaBlaCar.API.Controllers;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.AdminOrUser)]
    public class TripsController : CustomBaseController
    {
        private readonly ITripService _tripService;
        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("user-trips")]
        public async Task<IActionResult> GetUserTrips([FromQuery] int take,[FromQuery] int skip)
        {
           
            var res = await _tripService.GetUserTripsAsync(take, skip, UserId);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(Guid id)
        {
           
            var res = await _tripService.GetTripAsync(id, UserId);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<IActionResult> SearchTrips([FromBody]SearchTripDTO tripModel)
        {
            var res = await _tripService.SearchTripsAsync(tripModel);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody]CreateTripDTO  tripModel)
        {
            var res = await _tripService.AddTripAsync(tripModel, UserId);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var res = await _tripService.DeleteTripAsync(id, UserId);
            return NoContent();
        }
    }
}
