using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.NotificationModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
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
        public async Task<IEnumerable<UserModel>> GetRequestsAsync(ModelStatus status)
        {
            var users = _mapper.Map<IEnumerable<UserModel>>(await _unitOfWork.Users.GetAsync(null,
                x=>
                    x.Include(x=>x.Cars.Where(x=>x.CarStatus == (Status)status)).ThenInclude(x => x.CarDocuments),
                x=>x.UserStatus == (Status)status || x.Cars.Any(c=>c.CarStatus == (Status)status)));
            return users;

        }
      
        public async Task<UserModel> GetUserRequestsAsync(Guid id)
        {
            var user = _mapper.Map<UserModel>(await _unitOfWork.Users.GetAsync(
                x => x.Include(x => x.UserDocuments)
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.CarDocuments)
                    .Include(x=>x.Trips)
                    .Include(x=>x.TripUsers),
                x => x.Id == id));
            if(user.UserImg != null)
                user.UserImg = user.UserImg.Insert(0, _hostSettings.Host);

            user.UserDocuments = user.UserDocuments.Select(x =>
            {
                x.DrivingLicense = _hostSettings.Host + x.DrivingLicense;
                return x;

            }).ToList();

            user.Cars = user.Cars.Select(x =>
            {
                x.CarDocuments.Select(c =>
                {
                    c.TechPassport = _hostSettings.Host + c.TechPassport;
                    return c;
                }).ToList();
                return x;
            }).ToList();

            return user;
        }
        public async Task<bool> ChangeUserStatusAsync(ChangeUserStatus changeUserStatus, ClaimsPrincipal principal)
        {
            var user = _mapper.Map<UserModel>(
                await _unitOfWork.Users.GetAsync(null, x => x.Id == changeUserStatus.UserId));

            if (user is null) throw new Exception("User not found");
            user.UserStatus = changeUserStatus.Status;

            _unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            _notificationService.CreateNotificationAsync(
                new NotificationViewModel()
                {
                    NotificationStatus = NotificationModelStatus.SpecificUser,
                    Text = $"Your druving license status changed - {user.UserStatus}",
                    User = user
                }, principal);
            return await _unitOfWork.SaveAsync(changedBy);
        }
        public async Task<bool> ChangeCarStatusAsync(ChangeCarStatus changeCarStatus, ClaimsPrincipal principal)
        {
            var car = _mapper.Map<CarModel>
                (await _unitOfWork.Cars.GetAsync(null, x=>x.Id == changeCarStatus.CarId));

            if (car is null) throw new Exception("Car not found");
            car.CarStatus = changeCarStatus.Status;

            _unitOfWork.Cars.Update(_mapper.Map<Car>(car));

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            _notificationService.CreateNotificationAsync(
                new NotificationViewModel()
                {
                    NotificationStatus = NotificationModelStatus.SpecificUser,
                    Text = $"Your car {car.RegistNum} status changed - {car.CarStatus}",
                    UserId = car.UserId,
                }, principal);
            return await _unitOfWork.SaveAsync(changedBy);
        }
    }
}
