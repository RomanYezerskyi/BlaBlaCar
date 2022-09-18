using BlaBlaCar.API;
using BlaBlaCar.API.Controllers;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.Services.TripServices;
using IdentityModel;

namespace BlaBlaCar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.UserRole)]
    public class CarController : CustomBaseController
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCars()
        {
            var res = await _carService.GetUserCarsAsync(UserId);
            return Ok(res);
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
            await _carService.AddCarAsync(carModel, UserId);
            return NoContent();
        }
        [HttpPut("update-car")]
        public async Task<IActionResult> UpdateCar([FromForm] UpdateCarDTO carModel)
        {
            var res = await _carService.UpdateCarAsync(carModel, UserId);
            return NoContent();
        }
        [HttpPut("update-doc")]
        public async Task<IActionResult> UpdateCarDocuments([FromForm] UpdateCarDocumentsDTO carModel)
        {
            var res = await _carService.UpdateCarDocumentsAsync(carModel, UserId);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var userId = Guid.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);
            var res = await _carService.DeleteCarAsync(id, userId);
            return NoContent();
        }
    }
}
