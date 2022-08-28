using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlaBlaCar.BL.Services.Admin
{
    public class AdminService: IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HostSettings _hostSettings;
        private readonly INotificationService _notificationService;
        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<HostSettings> hostSettings, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<IEnumerable<UserDTO>> GetRequestsAsync(UserDTOStatus status)
        {
            var users = _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.Users.GetAsync(null,
                x=>
                    x.Include(x=>x.Cars.Where(x=>x.CarStatus == (Status)status)).ThenInclude(x => x.CarDocuments),
                x=>x.UserStatus == (Status)status || x.Cars.Any(c=>c.CarStatus == (Status)status)));
            if (!users.Any())
                throw new NotFoundException(nameof(UserDTO));
            return users;

        }
      
        public async Task<UserDTO> GetUserRequestsAsync(Guid id)
        {
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(
                x => x.Include(x => x.UserDocuments)
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.CarDocuments)
                    .Include(x=>x.Trips)
                    .Include(x=>x.TripUsers),
                x => x.Id == id));
            if (user is null)
                throw new NotFoundException(nameof(UserDTO));

            if (user.UserImg != null)
                user.UserImg = user.UserImg.Insert(0, _hostSettings.CurrentHost);

            user.UserDocuments = user.UserDocuments.Select(x =>
            {
                x.DrivingLicense = _hostSettings.CurrentHost + x.DrivingLicense;
                return x;

            }).ToList();

            user.Cars = user.Cars.Select(x =>
            {
                x.CarDocuments.Select(c =>
                {
                    c.TechPassport = _hostSettings.CurrentHost + c.TechPassport;
                    return c;
                }).ToList();
                return x;
            }).ToList();

            return user;
        }
        public async Task<bool> ChangeUserStatusAsync(ChangeUserStatusDTO changeUserStatus, ClaimsPrincipal principal)
        {
            var user = _mapper.Map<UserDTO>(
                await _unitOfWork.Users.GetAsync(null, x => x.Id == changeUserStatus.UserId));

            if (user is null) throw new NotFoundException(nameof(UserDTO));
            user.UserStatus = changeUserStatus.Status;

            _unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationDTOStatus.SpecificUser,
                    Text = $"Your driving license status changed - {user.UserStatus}",
                    UserId = user.Id
                });
            return await _unitOfWork.SaveAsync(changedBy);
        }
        public async Task<bool> ChangeCarStatusAsync(ChangeCarStatusDTO changeCarStatus, ClaimsPrincipal principal)
        {
            var car = _mapper.Map<CarDTO>
                (await _unitOfWork.Cars.GetAsync(null, x=>x.Id == changeCarStatus.CarId));

            if (car is null) throw new NotFoundException(nameof(CarDTO));
            car.CarStatus = changeCarStatus.Status;

            _unitOfWork.Cars.Update(_mapper.Map<Car>(car));

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationDTOStatus.SpecificUser,
                    Text = $"Your car {car.RegistNum} status changed - {car.CarStatus}",
                    UserId = car.UserId,
                });
            return await _unitOfWork.SaveAsync(changedBy);
        }
    }
}
