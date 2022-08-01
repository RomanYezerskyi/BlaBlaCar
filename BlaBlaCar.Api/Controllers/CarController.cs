using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.CarModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;

namespace BlaBlaCar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarsId()
        {
            try
            {
                var res = await _carService.GetUserCars(User);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] AddNewCarModel carModel)
        {
            try
            {
                if (carModel == null)
                    return BadRequest();
                var res = await _carService.AddCarAsync(carModel, User);
                if (res) return Ok("Added Successfully");
                return BadRequest("Fail");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
