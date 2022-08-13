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

        [HttpDelete]
        public async Task<IActionResult> DeleteBookedTrip(IEnumerable<TripUserViewModel> tripUser)
        {
            try
            {
                //var res = await _tripService.DeleteBookedTripAsync(id);
                //if (res) return Ok(new { Result = res });
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
