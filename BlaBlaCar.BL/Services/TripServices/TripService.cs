using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BlaBlaCar.DAL.Entities.TripEntities;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using Hangfire;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HostSettings _hostSettings;
        private readonly INotificationService _notificationService;
        private readonly IBackgroundJobClient _backgroundJobs;
        public TripService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsSnapshot<HostSettings> hostSettings, 
            INotificationService notificationService, 
            IBackgroundJobClient backgroundJobs)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _backgroundJobs = backgroundJobs;
            _hostSettings = hostSettings.Value;
        }
        public async Task<TripDTO> GetTripAsync(Guid id, Guid currentUserId)
        {
            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == id);
            var trip = _mapper.Map<Trip, TripDTO>(await _unitOfWork.Trips.GetAsync(
                                        x => x.Include(y => y.AvailableSeats)
                                                                        .ThenInclude(x => x.Seat)
                                                                        .Include(x=>x.User)
                                                                        .Include(x=>x.Car)
                                                                        .ThenInclude(x => x.Seats)
                                                                        .Include(
                                                                            x=>x.TripUsers.Where(x=>x.UserId == currentUserId)),
                                                                    x => x.Id == id));
            if (trip is null)
                throw new NotFoundException("Trip");

            trip.AvailableSeats.Select(x =>
            {
                if (usersBookedTrip.Any(y => y.SeatId == x.SeatId))
                {
                    x.AvailableSeatsType = AvailableSeatTypeDTO.Booked;
                }
                return x;
            }).ToList();
            if (trip.User != null && trip.User.UserImg != null) trip.User.UserImg = _hostSettings.CurrentHost + trip.User.UserImg;

            if (trip.Car != null)
                trip.Car.CarDocuments = trip.Car.CarDocuments.Select(c =>
                {
                    c.TechnicalPassport = _hostSettings.CurrentHost + c.TechnicalPassport;
                    return c;
                }).ToList();

            if (trip.UserId == currentUserId) trip.UserPermission = UserPermission.Owner;
            else if(trip.TripUsers != null 
                    && trip.TripUsers.Any(u=>u.UserId == currentUserId)) trip.UserPermission = UserPermission.СanSeeTheOwnersData;
            else trip.UserPermission = UserPermission.OnlyBook;
            return trip;

        }

        public async Task<UserTripsDTO> GetUserTripsAsync(int take, int skip,
            Guid currentUserId)
        {
            var trips = _mapper.Map<IEnumerable<TripDTO>>
                (await _unitOfWork.Trips.GetAsync(orderBy:null, 
                    includes:x=>x.Include(i=>i.Car)
                        .Include(x=>x.AvailableSeats)
                        .Include(i=>i.TripUsers).ThenInclude(i=>i.User)
                        .Include(i => i.TripUsers).ThenInclude(x=>x.Seat), 
                    filter:x=>x.UserId == currentUserId, 
                    take: take,
                    skip: skip));

            if (!trips.Any()) throw new NoContentException();

            var tripsCount =await _unitOfWork.Trips.GetCountAsync(x=>x.UserId == currentUserId);

            trips = trips.Select(t =>
            {
                if (t.Car != null)
                {
                    t.Car.CarDocuments = t.Car.CarDocuments.Select(c =>
                    {
                        if (c.TechnicalPassport != null)
                            c.TechnicalPassport = _hostSettings.CurrentHost + c.TechnicalPassport;
                        return c;
                    }).ToList();
                }

                t.TripUsers = t.TripUsers.Select(tu =>
                {
                    if(tu.User.UserImg != null)
                        tu.User.UserImg = _hostSettings.CurrentHost + tu.User.UserImg;
                    return tu;
                }).ToList();
                return t;
            });

            var listGroupByUsers = trips.Select(x => x.TripUsers
                .GroupBy(x => x.UserId)
                .Select(x => new GetBookedTripUsersDTO
                {
                    UserId = x.Key,
                    User = x.Select(u => u.User).FirstOrDefault(),
                    Seats = x.Select(x => x.Seat).ToList(),
                    
                }));

            var groupedList = _mapper.Map<IEnumerable<GetTripWithTripUsersDTO>>(trips);
            groupedList = groupedList.Select(trip =>
            {
                trip.BookedTripUsers = new List<GetBookedTripUsersDTO>();
                foreach (var listUsers in listGroupByUsers)
                {
                    foreach (var users in listUsers)
                    {
                        if(users.User.TripUsers?.First().TripId == trip.Id)
                            trip.BookedTripUsers.Add(users);
                    }
                }
                return trip;
            });

            var result = new UserTripsDTO() { Trips = groupedList, TotalTrips = tripsCount };
            return result;
        }
       
        public async Task<SearchTripsResponseDTO> SearchTripsAsync(SearchTripDTO model)
        {
            Expression<Func<Trip, bool>> tripFilter = trip => trip.StartPlace.Contains(model.StartPlace)
                                                           && trip.EndPlace.Contains(model.EndPlace)
                                                           && trip.StartTime.Date == model.StartTime.Date
                                                           && trip.AvailableSeats.Count(s =>
                                                               trip.TripUsers.All(u => u.SeatId != s.SeatId)) >= model.CountOfSeats;
            Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null;
            switch (model.OrderBy)
            {
                case TripOrderBy.EarliestDepartureTime:
                    orderBy = trip => trip.OrderBy(t => t.StartTime);
                    break;
                case TripOrderBy.ShortestTrip:
                    orderBy = trip => trip.OrderBy(x => new {res= x.EndTime-x.StartTime });
                    break;
                case TripOrderBy.LowestPrice:
                    orderBy = trip => trip.OrderBy(t => t.PricePerSeat);
                    break;
                default:
                    orderBy = trip => trip.OrderBy(t => t.StartTime);
                    break;
            }
            var trips = await _unitOfWork.Trips.GetAsync(
                orderBy: orderBy,
                includes: x => x.Include(x => x.AvailableSeats)
                    .Include(x => x.TripUsers)
                    .Include(x=>x.User),
                filter: tripFilter,
                skip: model.Skip,
                take: model.Take);

            if (!trips.Any()) throw new NoContentException();

            trips = trips.Select(t =>
            {
                if(t.User.UserImg != null)
                    t.User.UserImg = _hostSettings.CurrentHost + t.User.UserImg;
                return t;
            });

            var result = new SearchTripsResponseDTO()
            {
                Trips = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripDTO>>(trips),
            };
            if (model.Skip == 0)
            {
                result.TotalTrips = await _unitOfWork.Trips.GetCountAsync(tripFilter);
            }
            return result;
        }

        public async Task<bool> AddTripAsync(CreateTripDTO newTripModel, Guid currentUserId)
        {
            var tripModel = _mapper.Map<CreateTripDTO, TripDTO>(newTripModel);
            tripModel.UserId = currentUserId;

            var trip = _mapper.Map<TripDTO, Trip>(tripModel);

            await _unitOfWork.Trips.InsertAsync(trip);
            var res =  await _unitOfWork.SaveAsync(currentUserId);
            if (res)
                _backgroundJobs.Schedule((() => _notificationService.GenerateFeedBackNotificationAsync(trip.Id)),
                    trip.EndTime);
            return res;

        }

        public async Task<bool> DeleteTripAsync(Guid id, Guid currentUserId)
        {
            var trip = await _unitOfWork.Trips.GetAsync(
                includes: x=>x.Include(x=>x.TripUsers), 
                filter: x => x.Id == id);
            if (trip is null)
                throw new NotFoundException(nameof(TripDTO));
            //var tripUsers = await _unitOfWork.TripUser.GetAsync(null, null, x => x.TripId == id);
            //trip.tripUsers.ToList()
            trip.TripUsers.ToList().ForEach(u =>
            {
                _notificationService.GenerateNotificationAsync(new CreateNotificationDTO()
                {
                    UserId = u.UserId,
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"The trip {trip.StartPlace} - {trip.EndPlace} was cancelled!"
                });
            });

            //if (tripUsers != null) _unitOfWork.TripUser.Delete(tripUsers);
            _unitOfWork.Trips.Delete(trip);

            return await _unitOfWork.SaveAsync(currentUserId);
        }
    }
}
