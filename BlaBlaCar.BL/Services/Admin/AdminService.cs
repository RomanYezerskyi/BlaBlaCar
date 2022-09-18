using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.AdminDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services.TripServices;
using BlaBlaCar.BL.ViewModels.AdminViewModels;
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
        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<HostSettings> hostSettings, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<UserRequestsViewModel> GetRequestsAsync(int take, int skip, UserStatusDTO status)
        {
            var users = _mapper.Map<IEnumerable<UserDTO>>(
                await _unitOfWork.Users.GetAsync(
                    orderBy:null,
                    includes:x=>
                        x.Include(x=> x.Cars.Where(x=>x.CarStatus == (Status)status))
                            .ThenInclude(x => x.CarDocuments),
                filter:x=>x.UserStatus == (Status)status || x.Cars.Any(c=>c.CarStatus == (Status)status)));
            if (!users.Any())
                throw new NotFoundException(nameof(UserDTO));
            var usersCount = await _unitOfWork.Users.GetCountAsync(x=>x.UserStatus == (Status)status 
                                                                || x.Cars.Any(c => c.CarStatus == (Status)status));

            var result = new UserRequestsViewModel()
            {
                Users = users,
                TotalRequests = usersCount
            };
            return result;

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
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
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
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"Your car {car.RegistNum} status changed - {car.CarStatus}",
                    UserId = car.UserId,
                });
            return await _unitOfWork.SaveAsync(changedBy);
        }

        public async Task<AdminStaticViewModel> GetStatisticsDataAsync(DateTimeOffset searchDate)
        {
            var users = _mapper.Map<IEnumerable<UsersStatisticsDTO>>(
                await _unitOfWork.Users.GetAsync(null, null, 
                    x=>x.CreatedAt. Value.Month == searchDate.Month));
            var groupedUsers = users.GroupBy(x => x.CreatedAt.Value.Date);
            var cars = _mapper.Map<IEnumerable<CarStatisticsDTO>>(
                await _unitOfWork.Cars.GetAsync(null, null, x=> x.CreatedAt.Value.Month == searchDate.Month));
            var groupedCars = cars.GroupBy(x => x.CreatedAt.Value.Date);
            var trips = _mapper.Map<IEnumerable<TripsStatisticsDTO>>(
                await _unitOfWork.Trips.GetAsync(null, null,
                    x=>x.CreatedAt.Value.Month == searchDate.Month));
            var groupedWeekTrips = trips.GroupBy(x => x.CreatedAt.Value.DayOfWeek);
            var groupedTrips = trips.GroupBy(x => x.CreatedAt.Value.Date);
            var res = new AdminStaticViewModel()
            {
                UsersStatisticsCount = groupedUsers.Select(x=>x.ToList().Count()),
                UsersDateTime = groupedUsers.Select(x=>x.Key),

                CarsStatisticsCount = groupedCars.Select(x=>x.ToList().Count),
                CarsDateTime = groupedCars.Select(x=>x.Key),

                TripsStatisticsCount = groupedTrips.Select(x=>x.ToList().Count),
                TripsDateTime = groupedTrips.Select(x=>x.Key),

                WeekStatisticsTripsCount = groupedWeekTrips.Select(x=>x.ToList().Count),
                WeekTripsDateTime = groupedWeekTrips.Select(x=>x.Key)
            };
            return res;
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
