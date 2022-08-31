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
    [Authorize]
    public class BookedTripController : ControllerBase
    {
        private readonly IBookedTripsService _tripService;
        public BookedTripController(IBookedTripsService tripService)
        {
            _tripService = tripService;
        }
        [HttpPost]
        public async Task<IActionResult> BookTrip([FromBody] AddNewBookTripDTO bookedTrip)
        {
            
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));

            var res = await _tripService.AddBookedTripAsync(bookedTrip, User);
            if (res) return Ok(new {Result = "Added Successfully"});
            return BadRequest("Fail");
           
        }
        [HttpGet("trips/{take}/{skip}")]
        public async Task<IActionResult> GerUserBookedTrips(int take, int skip)
        {
            
            var res = await _tripService.GetUserBookedTripsAsync(take, skip, User);
            if (res != null) return Ok(res);
            return BadRequest("Fail");
            
        }
        [HttpDelete("trip")]
        public async Task<IActionResult> DeleteBookedTrip(DeleteTripUserDTO trip)
        {
           
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));

            var res = await _tripService.DeleteBookedTripAsync(trip, User);
            if (res) return Ok(new { Result = res });
            return BadRequest("Fail");
          
        }
        [HttpDelete("seat")]
        public async Task<IActionResult> DeleteBookedSeat(UpdateTripUserDTO tripUser)
        {
            
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _tripService.DeleteBookedSeatAsync(tripUser, User);
            if (res) return Ok(new { Result = res });
            return BadRequest("Fail");
          
        }
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUserFromTrip(UpdateTripUserDTO tripUser)
        {
            
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _tripService.DeleteUserFromTripAsync(tripUser, User);
            if (res) return Ok(new { Result = res });
            return BadRequest("Fail");
           
        }
    }
}
