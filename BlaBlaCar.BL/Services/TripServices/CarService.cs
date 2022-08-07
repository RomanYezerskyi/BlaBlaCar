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
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class CarService : ICarService
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
            //if (user.UserStatus == UserModelStatus.WithoutCar) user.UserStatus = UserModelStatus.Requested;

            if (carModel.TechPassportFile.Length > 0)
            {
                var folderName = Path.Combine("DriverDocuments", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = ContentDispositionHeaderValue.Parse(carModel.TechPassportFile.ContentDisposition).FileName.Trim('"').Split(".");
                var newFileName = new string(Guid.NewGuid() + "." + fileName.Last());
                var fullPath = Path.Combine(pathToSave, newFileName);
                var dbPath = Path.Combine(folderName, newFileName);
                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    carModel.TechPassportFile.CopyTo(stream);
                }

                var newCar = _mapper.Map<NewCarViewModel, CarModel>(carModel);
                newCar.CarStatus = ModelStatus.Pending;
                newCar.TechPassport = dbPath;
                //newCar.UserId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
                _carSeatsService.AddSeatsToCarAsync(newCar, carModel.CountOfSeats);

                user.Cars.Add(newCar);

                _unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));
                //var car = _mapper.Map<Car>(newCar);
                //await _unitOfWork.Cars.InsertAsync(car);
                return await _unitOfWork.SaveAsync();
            }
            throw new Exception("Problems with file");
        }


    }
}
