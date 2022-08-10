using System.Net.Http.Headers;
using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICarSeatsService _carSeatsService;
        private readonly IFileService _fileService;
        public CarService(IUnitOfWork unitOfWork,
            IMapper mapper,
           IUserService userService, ICarSeatsService carSeatsService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _carSeatsService = carSeatsService;
            _fileService = fileService;
        }

        public async Task<IEnumerable<CarModel>> GetUserCarsAsync(ClaimsPrincipal principal)
        {
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot create trip!");


            var userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
            var userCars = _mapper.Map<IEnumerable<CarModel>>
                (await _unitOfWork.Cars.GetAsync(null, null, x => x.UserId == userId));
            return userCars;
        }

        public async Task<CarModel> GetCarByIdAsync(Guid id)
        {
            var car = _mapper.Map<CarModel>(await _unitOfWork.Cars.GetAsync(x => x.Include(z => z.Seats), x => x.Id == id));
            return car;
        }


        public async Task<bool> AddCarAsync(NewCarViewModel carModel, ClaimsPrincipal principal)
        {
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot create trip!");
            var userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
            var user = _mapper.Map<UserModel>(await _unitOfWork.Users.GetAsync(null,
                x => x.Id == userId));

            if (user.UserStatus == ModelStatus.Rejected) throw new Exception("This user cannot add car!");
            
            if (carModel.TechPassportFile.Any())
            {
                var newCar = _mapper.Map<NewCarViewModel, CarModel>(carModel);
                

                var files = await _fileService.FilesDbPathListAsync(carModel.TechPassportFile);

                newCar.CarDocuments = files.Select(f => new CarDocumentsModel() { Car = newCar, TechPassport = f }).ToList();
                newCar.CarStatus = ModelStatus.Pending;

                _carSeatsService.AddSeatsToCarAsync(newCar, carModel.CountOfSeats);

                newCar.UserId = userId;
                //user.Cars.Add(newCar);

                //_unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));

                var car = _mapper.Map<Car>(newCar);
                await _unitOfWork.Cars.InsertAsync(car);
                return await _unitOfWork.SaveAsync();
            }
            throw new Exception("Problems with file");
        }

       

    }
}
