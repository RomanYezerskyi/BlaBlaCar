using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.NotificationModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities.TripEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlaBlaCar.BL.Services.BookedTripServices
{
    public class BookedTripsService : IBookedTripsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly HostSettings _hostSettings;
        private readonly INotificationService _notificationService;
        public BookedTripsService(IUnitOfWork unitOfWork, IMapper mapper,IUserService userService, IOptionsSnapshot<HostSettings> hostSettings, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _notificationService = notificationService;
            _hostSettings = hostSettings.Value;
        }

        public async Task<IEnumerable<TripModel>> GetUserBookedTripsAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = Guid.Parse((ReadOnlySpan<char>)claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            var trip = _mapper.Map<IEnumerable<TripModel>>(await _unitOfWork.Trips.GetAsync(x => x.OrderByDescending(x => x.StartTime), 
                x =>
                    x.Include(x => x.TripUsers.Where(x => x.UserId == userId))
                        .ThenInclude(x => x.Seat)
                        .Include(x=>x.User),
                x => x.TripUsers
                    .Any(x => x.UserId == userId)));

            trip = trip.Select(t =>
            {
                t.User.UserImg = _hostSettings.CurrentHost + t.User.UserImg;
                return t;
            });
            return trip;
        }

        public async Task<bool> AddBookedTripAsync(AddNewBookTrip tripModel, ClaimsPrincipal principal)
        {

            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot book trip!");
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);


            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == tripModel.TripId);
            var checkIfSeatNotBooked = tripModel.BookedSeats.Any(x => usersBookedTrip.Any(y => y.SeatId == x.Id));
            if (checkIfSeatNotBooked) throw new Exception("This seats already booked!");

            var listOfSeats = new List<TripUserModel>();
            for (int i = 0; i < tripModel.RequestedSeats; i++)
            {
                var userTripModel = _mapper.Map<AddNewBookTrip, TripUserModel>(tripModel);

                userTripModel.UserId = userId;
                userTripModel.SeatId = tripModel.BookedSeats.ElementAt(i).Id;
                listOfSeats.Add(userTripModel);
            }
            await _unitOfWork.TripUser.InsertRangeAsync(_mapper.Map<IEnumerable<TripUser>>(listOfSeats));

            var trip = await _unitOfWork.Trips.GetAsync(null, x => x.Id == tripModel.TripId);
            var userName = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName);

            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationViewModel()
                {
                    NotificationStatus = NotificationModelStatus.SpecificUser,
                    Text = $"{trip.StartPlace} - {trip.EndPlace}" +
                           $"\nUser {userName} joined your trip",
                    UserId = trip.UserId,
                });

            return await _unitOfWork.SaveAsync(userId);
        }

        public async Task<bool> DeleteBookedTripAsync(DeleteTripUserViewModel tripModel, ClaimsPrincipal principal)
        {
            var trip = _mapper.Map<IEnumerable<TripUserModel>>(await _unitOfWork.TripUser.GetAsync(null, null, x =>
                x.TripId == tripModel.Id && x.UserId == tripModel.TripUsers.First().UserId));
            if (trip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));

            var userName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationViewModel()
                {
                    NotificationStatus = NotificationModelStatus.SpecificUser,
                    Text = $"{tripModel.StartPlace} - {tripModel.EndPlace} " +
                           $"The {userName} canceled all reservations",
                    UserId = tripModel.UserId,
                });

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }

        public async Task<bool> DeleteBookedSeatAsync(TripUserViewModel tripUserModel, ClaimsPrincipal principal)
        {
            var userTrip = _mapper.Map<TripUserModel>(await _unitOfWork.TripUser.GetAsync(null, x =>
                x.TripId == tripUserModel.TripId && x.UserId == tripUserModel.UserId && x.SeatId == tripUserModel.SeatId));
            if (userTrip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<TripUser>(userTrip));

            var trip = _mapper.Map<TripModel>(
                await _unitOfWork.Trips.GetAsync(
                    null,
                    x => x.Id == tripUserModel.TripId));
            var seat = _mapper.Map<SeatModel>(
                await _unitOfWork.CarSeats.GetAsync(null, x => x.Id == tripUserModel.SeatId));
            var userName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationViewModel()
                {
                    NotificationStatus = NotificationModelStatus.SpecificUser,
                    Text = $"{trip.StartPlace} - {trip.EndPlace} " +
                           $"The {userName} canceled the reservation seat {seat.Num} ",
                    UserId = trip.UserId,
                });

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }

        public async Task<bool> DeleteUserFromTripAsync(TripUserViewModel tripUserModel, ClaimsPrincipal principal)
        {
            var trip = _mapper.Map<IEnumerable<TripUserModel>>(await _unitOfWork.TripUser.GetAsync(null,null, x =>
                x.TripId == tripUserModel.TripId && x.UserId == tripUserModel.UserId && x.TripId == tripUserModel.TripId));
            if (trip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }
    }
    
}
