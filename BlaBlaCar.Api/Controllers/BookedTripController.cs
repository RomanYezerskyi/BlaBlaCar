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
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));

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
        public async Task<IActionResult> DeleteBookedTrip(DeleteTripUserDTO trip)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));

                var res = await _tripService.DeleteBookedTripAsync(trip, User);
                if (res) return Ok(new { Result = res });
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("seat")]
        public async Task<IActionResult> DeleteBookedSeat(UpdateTripUserDTO tripUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));
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
        public async Task<IActionResult> DeleteUserFromTrip(UpdateTripUserDTO tripUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));
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
