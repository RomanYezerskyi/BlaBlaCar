using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
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

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            try
            {
                //var res = await _tripService.GetBookedTripsAsync();
                //if (res.Any()) return Ok(res);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> BookTrip([FromBody] AddNewBookTrip bookedTrip)
        {
            try
            {
                if (bookedTrip == null)
                    return BadRequest();
                var res = await _tripService.AddBookedTripAsync(bookedTrip, User);
                if (res) return Ok(new {Result = "Added Successfully"});
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("trips")]
        public async Task<IActionResult> GerUserBookedTrips()
        {
            try
            {
                var res = await _tripService.GetUserBookedTripsAsync(User);
                if (res != null) return Ok(res);
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("trip")]
        public async Task<IActionResult> DeleteBookedTrip(IEnumerable<TripUserViewModel> tripUser)
        {
            try
            {
                var res = await _tripService.DeleteBookedTripAsync(tripUser, User);
                if (res) return Ok(new { Result = res });
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("seat")]
        public async Task<IActionResult> DeleteBookedSeat(TripUserViewModel tripUser)
        {
            try
            {
                var res = await _tripService.DeleteBookedSeatAsync(tripUser, User);
                if (res) return Ok(new { Result = res });
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUserFromTrip(TripUserViewModel tripUser)
        {
            try
            {
                var res = await _tripService.DeleteUserFromTripAsync(tripUser, User);
                if (res) return Ok(new { Result = res });
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
