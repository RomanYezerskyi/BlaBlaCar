using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.BookTripModels;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.AdminOrUser)]
    public class BookedTripController : CustomBaseController
    {
        private readonly IBookedTripsService _tripService;
        public BookedTripController(IBookedTripsService tripService)
        {
            _tripService = tripService;
        }
        [HttpPost]
        public async Task<IActionResult> BookTrip([FromBody] NewBookTripModel bookedTrip)
        {

            var res = await _tripService.AddBookedTripAsync(bookedTrip, GetUserInformation());
            return NoContent();
        }
        [HttpGet("trips")]
        public async Task<IActionResult> GerUserBookedTrips([FromQuery] int take,[FromQuery] int skip)
        {
            
            var res = await _tripService.GetUserBookedTripsAsync(take, skip, UserId);
            return Ok(res);
        }
        [HttpGet("started")]
        public async Task<IActionResult> GerStartedTrips([FromQuery] int take,[FromQuery] int skip)
        {
            
            var res = await _tripService.GetUserStartedTripAsync(take, skip, UserId);
            return Ok(res);
        }
        [HttpDelete("trip")]
        public async Task<IActionResult> DeleteBookedTrip(DeleteTripUserDTO trip)
        {

            var res = await _tripService.DeleteBookedTripAsync(trip, UserId, UserName);
            return NoContent();
        }
        [HttpDelete("seat")]
        public async Task<IActionResult> DeleteBookedSeat(UpdateTripUserDTO tripUser)
        {
            var res = await _tripService.DeleteBookedSeatAsync(tripUser, UserId, UserName);
            return NoContent();
        }
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUserFromTrip(UpdateTripUserDTO tripUser)
        {
            var res = await _tripService.DeleteUserFromTripAsync(tripUser, UserId);
            return NoContent();
        }
    }
}
