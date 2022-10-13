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
using EmailService.Models;
using EmailService.Services;
namespace BlaBlaCar.BL.Services.BookedTripServices
{
    public class BookedTripsService : IBookedTripsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HostSettings _hostSettings;
        private readonly INotificationService _notificationService;
        private readonly IMapService _mapService;
        private readonly IEmailSender _emailSender;
        public BookedTripsService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IOptionsSnapshot<HostSettings> hostSettings, 
            INotificationService notificationService, 
            IMapService mapService, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _mapService = mapService;
            _emailSender = emailSender;
            _hostSettings = hostSettings.Value;
            
        }

        public async Task<UserBookedTripsDTO> GetUserBookedTripsAsync(int take, int skip, Guid currentUserId)
        {
            

            var trips = _mapper.Map<IEnumerable<TripDTO>>(
                await _unitOfWork.Trips.GetAsync(
                    orderBy:x => x.OrderByDescending(x => x.StartTime), 
                includes:x =>
                    x.Include(x => x.TripUsers.Where(x => x.UserId == currentUserId))
                        .ThenInclude( x=> x.Seat)
                        .Include(x=>x.User),
                filter:x => x.TripUsers
                    .Any(user => user.UserId == currentUserId),
                    take:take,
                    skip:skip));

            if (!trips.Any()) return null;
                var tripsCount = await _unitOfWork.Trips.GetCountAsync(x => x.TripUsers
                .Any(user => user.UserId == currentUserId));

            trips = trips.Select(t =>
            {
                if(t.User?.UserImg != null)
                    t.User.UserImg = _hostSettings.CurrentHost + t.User.UserImg;
                return t;
            });
            var result = new UserBookedTripsDTO()
            {
                TotalTrips = tripsCount,
                Trips = trips
            };
            return result;
        }

        public async Task<bool> AddBookedTripAsync(NewBookTripModel tripModel, UserDTO currentUser)
        {
            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == tripModel.TripId);
            var checkIfSeatNotBooked = tripModel.BookedSeats.Any(x => usersBookedTrip.Any(y => y.SeatId == x.Id));
            if (checkIfSeatNotBooked) throw new PermissionException("This seats already booked!");

            var listOfSeats = new List<TripUserDTO>();
            for (int i = 0; i < tripModel.RequestedSeats; i++)
            {
                var userTripModel = _mapper.Map<NewBookTripModel, TripUserDTO>(tripModel);

                userTripModel.UserId = currentUser.Id;
                userTripModel.SeatId = tripModel.BookedSeats.ElementAt(i).Id;
                listOfSeats.Add(userTripModel);
            }
            await _unitOfWork.TripUser.InsertRangeAsync(_mapper.Map<IEnumerable<TripUser>>(listOfSeats));

            var trip = await _unitOfWork.Trips.GetAsync(x=>x.Include(x=>x.User), x => x.Id == tripModel.TripId);

            var startPlace = await _mapService.GetPlaceInformation(trip.StartLocation.X, trip.StartLocation.Y);
            var endPlace = await _mapService.GetPlaceInformation(trip.EndLocation.X, trip.EndLocation.Y);
            if (startPlace == null || endPlace == null) throw new Exception("Places not found!");

            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"{startPlace?.FeaturesList.First().Properties.Formatted} - {endPlace?.FeaturesList.First().Properties.Formatted}" +
                           $"\n{currentUser.FirstName} joined your trip",
                    UserId = trip.UserId,
                });
            
            var result = await _unitOfWork.SaveAsync(currentUser.Id);
            if (result)
            {
                var user = await 
                    _unitOfWork.Users.GetAsync(null, x => x.Id == currentUser.Id);
                var message = new Message(new string[] { user.Email },
                    "Order confirmation", $"{startPlace?.FeaturesList.First().Properties.Formatted}" +
                                          $" - {endPlace?.FeaturesList.First().Properties.Formatted}" +
                                          $"\n This is your confirmation, show it to the driver - {trip.User.FirstName} " +
                                          $"on {trip.StartTime}");
                await _emailSender.SendEmailAsync(message);
            }

            return result;
        }

        public async Task<bool> DeleteBookedTripAsync(DeleteTripUserDTO tripModel, Guid currentUserId, string userName)
        {
            var trip = _mapper.Map<IEnumerable<TripUserDTO>>(await _unitOfWork.TripUser.GetAsync(null, null, x =>
                x.TripId == tripModel.Id && x.UserId == tripModel.TripUsers.First().UserId));
            if (trip is null)
                throw new NotFoundException(nameof(TripUserDTO));
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));

            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"{tripModel.StartPlace} - {tripModel.EndPlace} " +
                           $"The {userName} canceled all reservations",
                    UserId = tripModel.UserId,
                });

            return await _unitOfWork.SaveAsync(currentUserId);
        }

        public async Task<bool> DeleteBookedSeatAsync(UpdateTripUserDTO tripUserModel, Guid currentUserId, string userName)
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

            var startPlace = await _mapService.GetPlaceInformation(trip.StartLocation.X, trip.StartLocation.Y);
            var endPlace = await _mapService.GetPlaceInformation(trip.EndLocation.X, trip.EndLocation.Y);
            await _notificationService.GenerateNotificationAsync(
                new CreateNotificationDTO()
                {
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"{startPlace?.FeaturesList.First().Properties.Formatted} - {endPlace?.FeaturesList.First().Properties.Formatted} " +
                           $"The {userName} canceled the reservation seat {seat.SeatNumber} ",
                    UserId = trip.UserId,
                });
            return await _unitOfWork.SaveAsync(currentUserId);
        }

        public async Task<bool> DeleteUserFromTripAsync(UpdateTripUserDTO tripUserModel, Guid currentUserId)
        {
            var trip = _mapper.Map<IEnumerable<TripUserDTO>>(await _unitOfWork.TripUser.GetAsync(null,null, x =>
                x.TripId == tripUserModel.TripId && x.UserId == tripUserModel.UserId && x.TripId == tripUserModel.TripId));
            if (trip is null)
                throw new NotFoundException(nameof(TripUserDTO));
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));

            return await _unitOfWork.SaveAsync(currentUserId);
        }
    }
    
}
