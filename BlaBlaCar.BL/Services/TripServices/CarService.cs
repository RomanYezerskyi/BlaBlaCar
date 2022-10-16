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
        private readonly ICarSeatsService _carSeatsService;
        private readonly IFileService _fileService;
        private readonly HostSettings _hostSettings;
        public CarService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ICarSeatsService carSeatsService, 
            IFileService fileService,
            IOptionsSnapshot<HostSettings> hostSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _carSeatsService = carSeatsService;
            _fileService = fileService;
            _hostSettings = hostSettings.Value;
        }

        public async Task<IEnumerable<CarDTO>> GetUserCarsAsync(Guid currentUserId)
        {
            var userCars = _mapper.Map<IEnumerable<CarDTO>>
                (await _unitOfWork.Cars.GetAsync(null, x=>
                        x.Include(s=>s.Seats)
                            .Include(x=>x.CarDocuments), 
                    x => x.UserId == currentUserId));
            if (!userCars.Any()) return null;
            userCars = userCars.Select(c =>
            {
                c.CarDocuments = c.CarDocuments.Select(d =>
                {
                    d.TechnicalPassport = _hostSettings.CurrentHost + d.TechnicalPassport;
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
                throw new NotFoundException("Car");
            return car;
        }


        public async Task AddCarAsync(CreateCarDTO carModel, Guid currentUserId)
        {
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(null,
                x => x.Id == currentUserId)); 

            if (user.UserStatus == UserStatusDTO.Rejected) throw new PermissionException("This user cannot add car!");

            if (!carModel.TechPassportFile.Any()) throw new Exception("Problems with file");

            var newCar = _mapper.Map<CreateCarDTO, CarDTO>(carModel);
            var files = await _fileService.GetFilesDbPathAsync(carModel.TechPassportFile);

            newCar.CarDocuments = files.Select(f => new CarDocumentDTO() { Car = newCar, TechnicalPassport = f }).ToList();
            newCar.CarStatus = DTOs.CarDTOs.CarStatus.Pending;

            _carSeatsService.AddSeatsToCarAsync(newCar, carModel.CountOfSeats);

            newCar.UserId = currentUserId;
            
            var car = _mapper.Map<Car>(newCar);
            await _unitOfWork.Cars.InsertAsync(car); 
            await _unitOfWork.SaveAsync(currentUserId);
        }

        public async Task<bool> UpdateCarAsync(UpdateCarDTO carModel, Guid currentUserId)
        {
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(null,
                x => x.Id == currentUserId));

            if (user.UserStatus == UserStatusDTO.Rejected) throw new PermissionException("This user cannot add car!");
            var car = _mapper.Map<CarDTO>(await _unitOfWork.Cars.GetAsync(x=>
                x.Include(x=>x.Seats).Include(x=>x.CarDocuments), 
                x => x.Id == carModel.Id));
            if (car == null) throw new NotFoundException("This car");
            car.ModelName = carModel.ModelName;
            car.RegistrationNumber = carModel.RegistrationNumber;
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
            car.CarStatus = DTOs.CarDTOs.CarStatus.Pending;
            _unitOfWork.Cars.Update(_mapper.Map<Car>(car));
            return await _unitOfWork.SaveAsync(currentUserId);
        }

        public async Task<bool> UpdateCarDocumentsAsync(UpdateCarDocumentsDTO carModel, Guid currentUserId)
        {
            var car = _mapper.Map<CarDTO>(await _unitOfWork.Cars.GetAsync(x =>
                    x.Include(x => x.Seats).Include(x => x.CarDocuments),
                x => x.Id == carModel.Id));
            if (car == null) throw new NotFoundException("This car");

            if (carModel.TechnicalPassportFile != null && carModel.TechnicalPassportFile.Any())
            {
                var files = await _fileService.GetFilesDbPathAsync(carModel.TechnicalPassportFile);
                var doc  = files.Select(f => new CarDocumentDTO() { CarId = car.Id, TechnicalPassport = f }).ToList();
                await _unitOfWork.CarDocuments.InsertRangeAsync(_mapper.Map<IEnumerable<CarDocuments>>(doc));
            }

            if (carModel.DeletedDocuments != null )
            {
                var doc = car.CarDocuments.Select(x =>
                {
                    return carModel.DeletedDocuments.Any(d => d.Contains(x.TechnicalPassport)) ? x : null;
                }).Where(x => x != null).ToList();
                _unitOfWork.CarDocuments.Delete(_mapper.Map<IEnumerable<CarDocuments>>(doc));
                _fileService.DeleteFilesFormApi(doc.Where(x => x != null).Select(x => x.TechnicalPassport));
            }

            return await _unitOfWork.SaveAsync(currentUserId);
        }

        public async Task<bool> DeleteCarAsync(Guid carId, Guid userId)
        {
            var car = await _unitOfWork.Cars.GetAsync(x=>
            x.Include(x=>x.Seats).ThenInclude(x=>x.AvailableSeats).Include(x=>x.Trips).Include(x=>x.CarDocuments)
                , x => x.Id == carId);

            var trips = await _unitOfWork.Trips.GetAsync(null, null, 
                x => x.CarId == car.Id
                && (x.StartTime >= DateTimeOffset.Now || x.EndTime >= DateTimeOffset.Now));
            if (trips.Any()) throw new Exception("You need to complete trips with this car");

            _unitOfWork.Cars.Delete(car);
            var result = await _unitOfWork.SaveAsync(userId);

            _fileService.DeleteFilesFormApi(car.CarDocuments.Select(x=>x.TechnicalPassport));
            return result;
        }
    }
}
