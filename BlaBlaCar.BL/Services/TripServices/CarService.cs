using System.Net.Http.Headers;
using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities.CarEntities;
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

        public async Task<IEnumerable<CarDTO>> GetUserCarsAsync(ClaimsPrincipal principal)
        {
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new PermissionException("This user not authorized!");


            var userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
            var userCars = _mapper.Map<IEnumerable<CarDTO>>
                (await _unitOfWork.Cars.GetAsync(null, x=>
                        x.Include(s=>s.Seats), 
                    x => x.UserId == Guid.Parse((ReadOnlySpan<char>)userId)));
            return userCars;
        }

        public async Task<CarDTO> GetCarByIdAsync(Guid id)
        {
            var car = _mapper.Map<CarDTO>(await _unitOfWork.Cars.GetAsync(
                x => x.Include(z => z.Seats), 
                x => x.Id == id));
            if (car is null)
                throw new NotFoundException(nameof(CarDTO));
            return car;
        }


        public async Task<bool> AddCarAsync(CreateCarDTO carModel, ClaimsPrincipal principal)
        {
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new PermissionException("This user cannot create trip!");
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(null,
                x => x.Id == userId)); 

            if (user.UserStatus == UserDTOStatus.Rejected) throw new PermissionException("This user cannot add car!");
            
            if (carModel.TechPassportFile.Any())
            {
                var newCar = _mapper.Map<CreateCarDTO, CarDTO>(carModel);
                

                var files = await _fileService.FilesDbPathListAsync(carModel.TechPassportFile);

                newCar.CarDocuments = files.Select(f => new CarDocumentDTO() { Car = newCar, TechPassport = f }).ToList();
                newCar.CarStatus = CarDTOStatus.Pending;

                _carSeatsService.AddSeatsToCarAsync(newCar, carModel.CountOfSeats);

                newCar.UserId = userId;
                
                var car = _mapper.Map<Car>(newCar);
                await _unitOfWork.Cars.InsertAsync(car);
                return await _unitOfWork.SaveAsync(userId);
            }
            throw new Exception("Problems with file");
        }

       

    }
}
