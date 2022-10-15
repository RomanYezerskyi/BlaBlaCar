using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.AdminDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services.TripServices;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Entities.TripEntities;
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
        public AdminService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IOptionsSnapshot<HostSettings> hostSettings, 
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<UserRequestsDTO> GetRequestsAsync(int take, int skip, UserStatusDTO status)
        {
            var users = _mapper.Map<IEnumerable<UserDTO>>(
                await _unitOfWork.Users.GetAsync(
                    orderBy:null,
                    includes:x=>
                        x.Include(x=> x.Cars.Where(x=>x.CarStatus == (Status)status))
                            .ThenInclude(x => x.CarDocuments),
                filter:x=>x.UserStatus == (Status)status || x.Cars.Any(c=>c.CarStatus == (Status)status)));
           
            
            
            if (!users.Any())
                return null;
            var usersCount = await _unitOfWork.Users.GetCountAsync(x=>x.UserStatus == (Status)status 
                                                                || x.Cars.Any(c => c.CarStatus == (Status)status));

            var result = new UserRequestsDTO()
            {
                Users = users,
                TotalRequests = usersCount
            };
            return result;

        }
      
       
        public async Task<bool> ChangeUserStatusAsync(ChangeUserStatusDTO changeUserStatus, Guid currentUserId)
        {
            var user = _mapper.Map<UserDTO>(
                await _unitOfWork.Users.GetAsync(null, x => x.Id == changeUserStatus.UserId));

            if (user is null) throw new NotFoundException(nameof(UserDTO));
            user.UserStatus = changeUserStatus.Status;

            _unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));

            var result = await _unitOfWork.SaveAsync(currentUserId);
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"Your driving license status changed - {user.UserStatus}",
                    UserId = user.Id
                }, currentUserId);
            return result;
        }
        public async Task<bool> ChangeCarStatusAsync(ChangeCarStatus changeCarStatus, Guid currentUserId)
        {
            var car = _mapper.Map<CarDTO>
                (await _unitOfWork.Cars.GetAsync(null, x=>x.Id == changeCarStatus.CarId));

            if (car is null) throw new NotFoundException(nameof(CarDTO));
            car.CarStatus = changeCarStatus.Status;

            _unitOfWork.Cars.Update(_mapper.Map<Car>(car));

            var result = await _unitOfWork.SaveAsync(currentUserId);
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"Your car {car.RegistrationNumber} status changed - {car.CarStatus}",
                    UserId = car.UserId,
                }, currentUserId);
            return result;
        }

        public async Task<AdminStatisticsDTO> GetStatisticsDataAsync(DateTimeOffset searchDate)
        {
            var users = _mapper.Map<IEnumerable<UsersStatisticsDTO>>(
                await _unitOfWork.Users.GetAsync(null, null, 
                    x=>x.CreatedAt.Month == searchDate.Month));
            var groupedUsers = users.GroupBy(x => x.CreatedAt.Value.Date.ToString("MM-dd"));
            var cars = _mapper.Map<IEnumerable<CarStatisticsDTO>>(
                await _unitOfWork.Cars.GetAsync(null, null, x=> x.CreatedAt.Month == searchDate.Month));
            var groupedCars = cars.GroupBy(x => x.CreatedAt.Value.Date.ToString("MM-dd"));

            var mondayDate = GetDayOfWeek(DateTimeOffset.Now, DayOfWeek.Monday);
            var sundayDate = GetDayOfWeek(DateTimeOffset.Now, DayOfWeek.Sunday);
            var trips = _mapper.Map<IEnumerable<TripDTO>>(
                await _unitOfWork.Trips.GetAsync(null, null,
                    x=>x.CreatedAt.Month == searchDate.Month));
            var groupedTrips = trips.GroupBy(x => x.CreatedAt.Date.ToString("MM-dd"));


            var groupedWeekTrips = trips.Where(x=> x.CreatedAt >= mondayDate || x.CreatedAt <= sundayDate)
                .GroupBy(x => x.CreatedAt.DayOfWeek.ToString());
          
            var res = new AdminStatisticsDTO()
            {
                UsersStatisticsCount = groupedUsers.Select(x=>x.ToList().Count),
                UsersDateTime = groupedUsers.Select(x=>x.Key),

                CarsStatisticsCount = groupedCars.Select(x=>x.ToList().Count),
                CarsDateTime = groupedCars.Select(x=>x.Key),

                TripsStatisticsCount = groupedTrips.Select(x=>x.ToList().Count),
                TripsDateTime = groupedTrips.Select(x=>x.Key),

                WeekStatisticsTripsCount = groupedWeekTrips.Select(x=>x.ToList().Count).ToList(),
                WeekTripsDateTime = groupedWeekTrips.Select(x=>x.Key).ToList()
            };
            return res;
        }

        public async Task<ShortStatisticsDTO> GetShortStatisticsDataAsync()
        {
            var mondayDate = GetDayOfWeek(DateTimeOffset.Now, DayOfWeek.Monday);
            var sundayDate = GetDayOfWeek(DateTimeOffset.Now, DayOfWeek.Sunday);
            var statistics = new ShortStatisticsDTO
            {
                UsersCount = await _unitOfWork.Users.GetCountAsync(null),
                CarsCount = await _unitOfWork.Cars.GetCountAsync(null),
                TripsCount = await _unitOfWork.Trips.GetCountAsync(null),
                WeekTripsCount = await _unitOfWork.Trips
                    .GetCountAsync(x=>x.CreatedAt >= mondayDate || x.CreatedAt <= sundayDate )
            };

            return statistics;
        }

        public DateTimeOffset GetDayOfWeek(DateTimeOffset currentDate, DayOfWeek day)
        {
            var diff = (7 + (currentDate.DayOfWeek - day)) % 7;
            return DateTimeOffset.Now.AddDays(-1 * diff).Date;
        }
        public async Task<IEnumerable<UsersStatisticsDTO>> GetTopUsersListAsync(int take, int skip, UsersListOrderByType orderBy)
        {
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> usersOrderBy = null;
            switch (orderBy)
            {
                case UsersListOrderByType.Trips:
                    usersOrderBy = user => 
                        user.OrderByDescending(u=>u.Trips.Count).ThenByDescending(u=>u.TripUsers.Count);
                    break;
                case UsersListOrderByType.UserTrips:
                    usersOrderBy = user =>
                        user.OrderByDescending(u => u.TripUsers.Count);
                    break;

                default:
                    usersOrderBy = user =>
                        user.OrderByDescending(u => u.Trips.Count).ThenByDescending(u => u.TripUsers.Count); ;
                    break;
            }
            var users = _mapper.Map<IEnumerable<UsersStatisticsDTO>>(
                await _unitOfWork.Users.GetAsync(
                    usersOrderBy, 
                    x=>x.Include(x=>x.Trips).Include(x=>x.TripUsers), 
                    null,
                    take:take,
                    skip:skip));
            if (!users.Any()) return null;
            users = users.Select(u =>
            {
                if (u.UserImg != null)
                {
                    u.UserImg = u.UserImg.Insert(0, _hostSettings.CurrentHost);
                }
                return u;
            });
            return users;
        }
    }
}
