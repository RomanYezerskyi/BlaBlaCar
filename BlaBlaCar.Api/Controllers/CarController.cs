using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.ModelStateValidationAttribute;
using BlaBlaCar.BL.Services.TripServices;
using IdentityModel;

namespace BlaBlaCar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ModelStateValidationActionFilter]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCars()
        {
            var res = await _carService.GetUserCarsAsync(User);
            if(res.Any())
                return Ok(res);
            return BadRequest("This user don't have a cars!");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(Guid id)
        {
            
            var res = await _carService.GetCarByIdAsync(id);
            return Ok(res);
           
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromForm] CreateCarDTO carModel)
        {
            await _carService.AddCarAsync(carModel, User);
            return NoContent();

        }
        [HttpPost("update-car")]
        public async Task<IActionResult> UpdateCar([FromForm] UpdateCarDTO carModel)
        {
            var res = await _carService.UpdateCarAsync(carModel, User);
            if (res) return Ok("Updated Successfully");
            return BadRequest("Fail");

        }
        [HttpPost("update-doc")]
        public async Task<IActionResult> UpdateCarDocuments([FromForm] UpdateCarDocumentsDTO carModel)
        {
            var res = await _carService.UpdateCarDocumentsAsync(carModel, User);
            if (res) return Ok("Updated Successfully");
            return BadRequest("Fail");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var userId = Guid.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);
            var res = await _carService.DeleteCarAsync(id, userId);
            if (res) return Ok(new { result = "Deleted Successfully" });
            return BadRequest("Fail");

        }
    }
}
