using System.Security.Claims;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("user-trips")]
        public async Task<IActionResult> GetUserTrips()
        {
            try
            {
                var res = await _tripService.GetUserTripsAsync(User);
                if (res.Any()) return Ok(res);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(Guid id)
        {
            try
            {
                var res = await _tripService.GetTripAsync(id);
                if (res != null) return Ok(res);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<IActionResult> SearchTrips([FromBody]SearchTripModel tripModel)
        {
            try
            {
                var res = await _tripService.SearchTripsAsync(tripModel);
                if(res.Any()) return Ok(res);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody]NewTripViewModel  tripModel)
        {
            try
            {
                if (tripModel == null)
                    return BadRequest();
                var res = await _tripService.AddTripAsync(tripModel, User);
                if (res) return Ok("Added Successfully");
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTrip([FromBody] TripModel tripModel)
        {
            try
            {
                if (tripModel == null)
                    return BadRequest();
                var res = await _tripService.UpdateTripAsync(tripModel);
                if (res) return Ok("Added Successfully");
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("trip/{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            try
            {
                var res = await _tripService.DeleteTripAsync(id);
                if (res) return Ok(new { result="Deleted Successfully"});
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
