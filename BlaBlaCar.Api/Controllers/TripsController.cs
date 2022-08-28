using System.Security.Claims;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.Interfaces;
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
        public async Task<IActionResult> SearchTrips([FromBody]SearchTripDTO tripModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));
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
        public async Task<IActionResult> CreateTrip([FromBody]CreateTripDTO  tripModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)));
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
        public async Task<IActionResult> UpdateTrip([FromBody] TripDTO tripModel)
        {
            try
            {
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
                var res = await _tripService.DeleteTripAsync(id, User);
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
