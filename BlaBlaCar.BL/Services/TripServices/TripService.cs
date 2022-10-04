using System.Globalization;
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
using NetTopologySuite.Geometries;
using BlaBlaCar.BL.Extensions;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using BlaBlaCar.BL.Services.MapsServices;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HostSettings _hostSettings;
        private readonly INotificationService _notificationService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IMapService _mapService;
        public TripService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsSnapshot<HostSettings> hostSettings, 
            INotificationService notificationService, 
            IBackgroundJobClient backgroundJobs, 
            IMapService mapService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _backgroundJobs = backgroundJobs;
            _mapService = mapService;
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
                (await _unitOfWork.Trips.GetAsync(orderBy:x=>x.OrderByDescending(x=>x.EndTime), 
                    includes:x=>x.Include(i=>i.Car)
                        .Include(x=>x.AvailableSeats)
                        .Include(i=>i.TripUsers).ThenInclude(i=>i.User)
                        .Include(i => i.TripUsers).ThenInclude(x=>x.Seat), 
                    filter:x=>x.UserId == currentUserId, 
                    take: take,
                    skip: skip));

            if (!trips.Any()) return null;

            var tripsCount =await _unitOfWork.Trips.GetCountAsync(x=>x.UserId == currentUserId);

            foreach (var trip in trips)
            {
                if (trip.TripUsers != null)
                    trip.TripUsers = trip.TripUsers.Select(u =>
                    {
                        if (u.User.UserImg != null)
                        {
                            u.User.UserImg = _hostSettings.CurrentHost + u.User.UserImg;
                        }

                        return u;
                    }).ToList();
            }

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

            Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = model.OrderBy switch
            {
                TripOrderBy.EarliestDepartureTime => trip => trip.OrderBy(t => t.StartTime),
                TripOrderBy.ShortestTrip => trip => trip.OrderBy(x => x.TripTime),
                TripOrderBy.LowestPrice => trip => trip.OrderBy(t => t.PricePerSeat),
                _ => trip => trip.OrderBy(t => t.StartTime)
            };

            var radius = 50000;

            var startLocationLat = model.StartLat.ToString(CultureInfo.InvariantCulture);
            var startLocationLon = model.StartLon.ToString(CultureInfo.InvariantCulture);
            var endLocationLat = model.EndLat.ToString(CultureInfo.InvariantCulture);
            var endLocationLon = model.EndLon.ToString(CultureInfo.InvariantCulture);

            var sqlRaw = $"SELECT * FROM [Trips] AS [t]" +
                         $"WHERE (CONVERT(date, [t].[StartTime]) = '{model.StartTime.Date:yyyy-MM-dd}') AND" +
                         $"([t].StartLocation.STDistance(geography::STGeomFromText('POINT({startLocationLat} {startLocationLon})', 4326)) < {radius}) " +
                         $" AND ([t].EndLocation.STDistance(geography::STGeomFromText('POINT({endLocationLat} {endLocationLon})', 4326)) < {radius})  AND" +
                         $" ((SELECT COUNT(*)FROM [AvailableSeats] AS [a] WHERE ([t].[Id] = [a].[TripId]) AND NOT EXISTS (SELECT 1 FROM [TripUsers] AS [t0]" +
                         $" WHERE ([t].[Id] = [t0].[TripId]) AND (([t0].[SeatId] = [a].[SeatId]) AND ([t0].[SeatId] IS NOT NULL)))) >= {model.CountOfSeats})";
            var trips = await _unitOfWork.Trips.GetFromSqlRowAsync(sqlRaw: sqlRaw,
                includes: x => x.Include(x => x.User)
                                                .Include(x=>x.AvailableSeats)
                                                .Include(x=>x.TripUsers), 
                orderBy:orderBy,
                skip: model.Skip,
                take: model.Take);

            if (!trips.Any()) return null;

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
            //if (model.Skip == 0)
            //{
            //    result.TotalTrips = 10; //await _unitOfWork.Trips.GetCountAsync(tripFilter);
            //}
            return result;
        }

        public async Task<bool> AddTripAsync(CreateTripDTO newTripModel, Guid currentUserId)
        {
            var tripModel = _mapper.Map<CreateTripDTO, TripDTO>(newTripModel);
            tripModel.UserId = currentUserId;
            
            var trip = _mapper.Map<TripDTO, Trip>(tripModel);
            trip.StartLocation = new Point(newTripModel.StartLat, newTripModel.StartLon) { SRID = 4326 };
            trip.EndLocation = new Point(newTripModel.EndLat, newTripModel.EndLon) { SRID = 4326 };
            //var distanceInMeters = seattle.ProjectTo(2855).Distance(trip.Location.ProjectTo(2855));

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

            var startPlace = await _mapService.GetPlaceInformation(trip.StartLocation.X, trip.StartLocation.Y);
            var endPlace = await _mapService.GetPlaceInformation(trip.EndLocation.X, trip.EndLocation.Y);
            trip.TripUsers.ToList().ForEach(u =>
            {
                _notificationService.GenerateNotificationAsync(new CreateNotificationDTO()
                {
                    UserId = u.UserId,
                    NotificationStatus = NotificationStatusDTO.SpecificUser,
                    Text = $"The trip {startPlace?.FeaturesList.First().Properties.Formatted} - {endPlace?.FeaturesList.First().Properties.Formatted} was cancelled!"
                });
            });

            //if (tripUsers != null) _unitOfWork.TripUser.Delete(tripUsers);
            _unitOfWork.Trips.Delete(trip);

            return await _unitOfWork.SaveAsync(currentUserId);
        }
    }
}
