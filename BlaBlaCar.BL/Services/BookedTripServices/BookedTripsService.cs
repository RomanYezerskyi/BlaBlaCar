using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.BookTripModels;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Interfaces;
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

        public async Task<IEnumerable<TripDTO>> GetUserBookedTripsAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            var trip = _mapper.Map<IEnumerable<TripDTO>>(await _unitOfWork.Trips.GetAsync(x => x.OrderByDescending(x => x.StartTime), 
                x =>
                    x.Include(x => x.TripUsers.Where(x => x.UserId == userId))
                        .ThenInclude(x => x.Seat)
                        .Include(x=>x.User),
                x => x.TripUsers
                    .Any(x => x.UserId == userId)));

            if (!trip.Any())
                throw new NotFoundException(nameof(TripDTO));

            trip = trip.Select(t =>
            {
                t.User.UserImg = _hostSettings.CurrentHost + t.User.UserImg;
                return t;
            });
            return trip;
        }

        public async Task<bool> AddBookedTripAsync(AddNewBookTripDTO tripModel, ClaimsPrincipal principal)
        {

            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new PermissionException("This user cannot book trip!");
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == tripModel.TripId);
            var checkIfSeatNotBooked = tripModel.BookedSeats.Any(x => usersBookedTrip.Any(y => y.SeatId == x.Id));
            if (checkIfSeatNotBooked) throw new PermissionException("This seats already booked!");

            var listOfSeats = new List<TripUserDTO>();
            for (int i = 0; i < tripModel.RequestedSeats; i++)
            {
                var userTripModel = _mapper.Map<AddNewBookTripDTO, TripUserDTO>(tripModel);

                userTripModel.UserId = userId;
                userTripModel.SeatId = tripModel.BookedSeats.ElementAt(i).Id;
                listOfSeats.Add(userTripModel);
            }
            await _unitOfWork.TripUser.InsertRangeAsync(_mapper.Map<IEnumerable<TripUser>>(listOfSeats));

            var trip = await _unitOfWork.Trips.GetAsync(null, x => x.Id == tripModel.TripId);
            var userName = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName);

            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationDTOStatus.SpecificUser,
                    Text = $"{trip.StartPlace} - {trip.EndPlace}" +
                           $"\nUser {userName} joined your trip",
                    UserId = trip.UserId,
                });

            return await _unitOfWork.SaveAsync(userId);
        }

        public async Task<bool> DeleteBookedTripAsync(DeleteTripUserDTO tripModel, ClaimsPrincipal principal)
        {
            var trip = _mapper.Map<IEnumerable<TripUserDTO>>(await _unitOfWork.TripUser.GetAsync(null, null, x =>
                x.TripId == tripModel.Id && x.UserId == tripModel.TripUsers.First().UserId));
            if (trip is null)
                throw new NotFoundException(nameof(TripUserDTO));
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));

            var userName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationDTOStatus.SpecificUser,
                    Text = $"{tripModel.StartPlace} - {tripModel.EndPlace} " +
                           $"The {userName} canceled all reservations",
                    UserId = tripModel.UserId,
                });

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }

        public async Task<bool> DeleteBookedSeatAsync(UpdateTripUserDTO tripUserModel, ClaimsPrincipal principal)
        {
            var userTrip = _mapper.Map<TripUserDTO>(await _unitOfWork.TripUser.GetAsync(null, x =>
                x.TripId == tripUserModel.TripId && x.UserId == tripUserModel.UserId && x.SeatId == tripUserModel.SeatId));
            if (userTrip is null)
                throw new NotFoundException(nameof(TripUserDTO));
            _unitOfWork.TripUser.Delete(_mapper.Map<TripUser>(userTrip));

            var trip = _mapper.Map<TripDTO>(
                await _unitOfWork.Trips.GetAsync(
                    null,
                    x => x.Id == tripUserModel.TripId));
            var seat = _mapper.Map<SeatDTO>(
                await _unitOfWork.CarSeats.GetAsync(null, x => x.Id == tripUserModel.SeatId));
            var userName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationDTOStatus.SpecificUser,
                    Text = $"{trip.StartPlace} - {trip.EndPlace} " +
                           $"The {userName} canceled the reservation seat {seat.Num} ",
                    UserId = trip.UserId,
                });

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }

        public async Task<bool> DeleteUserFromTripAsync(UpdateTripUserDTO tripUserModel, ClaimsPrincipal principal)
        {
            var trip = _mapper.Map<IEnumerable<TripUserDTO>>(await _unitOfWork.TripUser.GetAsync(null,null, x =>
                x.TripId == tripUserModel.TripId && x.UserId == tripUserModel.UserId && x.TripId == tripUserModel.TripId));
            if (trip is null)
                throw new NotFoundException(nameof(TripUserDTO));
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }
    }
    
}
