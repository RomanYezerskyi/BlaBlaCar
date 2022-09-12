using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.ModelStateValidationAttribute;

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
           
            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _carService.AddCarAsync(carModel, User);
            if (res) return Ok("Added Successfully");
            return BadRequest("Fail");
            
        }
        [HttpPost("update-car")]
        public async Task<IActionResult> UpdateCar([FromForm] UpdateCarDTO carModel)
        {

            //if (!ModelState.IsValid)
            //    throw new Exception(string.Join("; ", ModelState.Values
            //        .SelectMany(x => x.Errors)
            //        .Select(x => x.ErrorMessage)));
            var res = await _carService.UpdateCarAsync(carModel, User);
            if (res) return Ok("Updated Successfully");
            return BadRequest("Fail");

        }
        [HttpPost("update-doc")]
        public async Task<IActionResult> UpdateCarDocuments([FromForm] UpdateCarDocumentsDTO carModel)
        {

            if (!ModelState.IsValid)
                throw new Exception(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var res = await _carService.UpdateCarDocumentsAsync(carModel, User);
            if (res) return Ok("Updated Successfully");
            return BadRequest("Fail");

        }
    }
}
