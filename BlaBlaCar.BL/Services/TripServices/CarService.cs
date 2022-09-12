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
using Microsoft.Extensions.Options;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICarSeatsService _carSeatsService;
        private readonly IFileService _fileService;
        private readonly HostSettings _hostSettings;
        public CarService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserService userService, 
            ICarSeatsService carSeatsService, 
            IFileService fileService,
            IOptionsSnapshot<HostSettings> hostSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _carSeatsService = carSeatsService;
            _fileService = fileService;
            _hostSettings = hostSettings.Value;
        }

        public async Task<IEnumerable<CarDTO>> GetUserCarsAsync(ClaimsPrincipal principal)
        {
            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var userCars = _mapper.Map<IEnumerable<CarDTO>>
                (await _unitOfWork.Cars.GetAsync(null, x=>
                        x.Include(s=>s.Seats)
                            .Include(x=>x.CarDocuments), 
                    x => x.UserId == userId));

            userCars = userCars.Select(c =>
            {
                c.CarDocuments = c.CarDocuments.Select(d =>
                {
                    d.TechPassport = _hostSettings.CurrentHost + d.TechPassport;
                    return d;
                }).ToList();
                return c;
            });
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

        public async Task<bool> UpdateCarAsync(UpdateCarDTO carModel, ClaimsPrincipal principal)
        {
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(null,
                x => x.Id == userId));

            if (user.UserStatus == UserDTOStatus.Rejected) throw new PermissionException("This user cannot add car!");
            var car = _mapper.Map<CarDTO>(await _unitOfWork.Cars.GetAsync(x=>
                x.Include(x=>x.Seats).Include(x=>x.CarDocuments), 
                x => x.Id == carModel.Id));
            if (car == null) throw new NotFoundException("This car");
            car.ModelName = carModel.ModelName;
            car.RegistNum = carModel.RegistNum;
            car.CarType = carModel.CarType;
           
            //if (carModel.CountOfSeats > car.Seats.Count)
            //{
            //    _carSeatsService.AddSeatsToCarAsync(car, carModel.CountOfSeats - car.Seats.Count);
            //}
            //else if(carModel.CountOfSeats < car.Seats.Count)
            //{
            //    int take = car.Seats.Count - carModel.CountOfSeats;
            //    var seats = await _unitOfWork.CarSeats.GetAsync(
            //        x=>x.OrderByDescending(x=>x.Num), null, 
            //        x => x.CarId == car.Id, take:take);

            //    _unitOfWork.CarSeats.Delete(_mapper.Map<IEnumerable<Seat>>(seats));
            //}
            car.CarStatus = CarDTOStatus.Pending;
            _unitOfWork.Cars.Update(_mapper.Map<Car>(car));
            return await _unitOfWork.SaveAsync(userId);
        }

        public async Task<bool> UpdateCarDocumentsAsync(UpdateCarDocumentsDTO carModel, ClaimsPrincipal principal)
        {
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var car = _mapper.Map<CarDTO>(await _unitOfWork.Cars.GetAsync(x =>
                    x.Include(x => x.Seats).Include(x => x.CarDocuments),
                x => x.Id == carModel.Id));
            if (car == null) throw new NotFoundException("This car");

            if (carModel.TechPassportFile != null && carModel.TechPassportFile.Any())
            {
                var files = await _fileService.FilesDbPathListAsync(carModel.TechPassportFile);
                var doc  = files.Select(f => new CarDocumentDTO() { CarId = car.Id, TechPassport = f }).ToList();
                await _unitOfWork.CarDocuments.InsertRangeAsync(_mapper.Map<IEnumerable<CarDocuments>>(doc));
            }

            if (carModel.DeletedDocuments != null )
            {
                var doc = car.CarDocuments.Select(x =>
                {
                    return carModel.DeletedDocuments.Any(d => d.Contains(x.TechPassport)) ? x : null;
                }).Where(x => x != null).ToList();
                _unitOfWork.CarDocuments.Delete(_mapper.Map<IEnumerable<CarDocuments>>(doc));
                _fileService.DeleteFileFormApi(doc.Where(x => x != null).Select(x => x.TechPassport));
            }

            return await _unitOfWork.SaveAsync(userId);
        }
    }
}
