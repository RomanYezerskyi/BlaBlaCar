using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IEnumerable<TripModel>> GetTrips()
        {
            try
            {
                return await _tripService.GetTripsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<TripModel> GetTrips(int id)
        {
            try
            {
                return await _tripService.GetTripAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpGet("search")]
        public async Task<IEnumerable<TripModel>> GetTrips([FromQuery]TripModel tripModel)
        {
            try
            {
                return await _tripService.GetTripsAsync(tripModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody]TripModel  tripModel)
        {
            try
            {
                if (tripModel == null)
                    return BadRequest();
                var res = await _tripService.AddTripAsync(tripModel);
                if (res) return new JsonResult("Added Successfully");
                return new JsonResult("Fail");
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
                if (res) return new JsonResult("Added Successfully");
                return new JsonResult("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                var res = await _tripService.DeleteTripAsync(id);
                if (res) return new JsonResult("Deleted Successfully");
                return new JsonResult("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
