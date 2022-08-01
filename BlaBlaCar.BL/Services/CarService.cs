using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services
{
    public class CarService: ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICarSeatsService _carSeatsService;
        public CarService(IUnitOfWork unitOfWork,
            IMapper mapper,
           IUserService userService, ICarSeatsService carSeatsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _carSeatsService = carSeatsService;
        }

        public async Task<IEnumerable<CarModel>> GetUserCars(ClaimsPrincipal principal)
        {
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot create trip!");


            var userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
            var userCars = _mapper.Map<IEnumerable<CarModel>>
                (await _unitOfWork.Cars.GetAsync(null, null, x=>x.UserId == userId));
            return userCars;
        }


        public async Task<bool> AddCarAsync(AddNewCarModel carModel, ClaimsPrincipal principal)
        {
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot create trip!");

            var newCar = _mapper.Map<AddNewCarModel, CarModel>(carModel);
            newCar.UserId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;

            await _carSeatsService.AddSeatsToCarAsync(newCar, carModel.CountOfSeats);

            var car = _mapper.Map<Car>(newCar);
            await _unitOfWork.Cars.InsertAsync(car);
            return await _unitOfWork.SaveAsync();
        }

        
    }
}
