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

namespace BlaBlaCar.BL.Services.TripServices
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly HostSettings _hostSettings;
        private readonly INotificationService _notificationService;
        public TripService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserService userService, 
            IOptionsSnapshot<HostSettings> hostSettings, 
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _notificationService = notificationService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<TripDTO> GetTripAsync(Guid id)
        {
            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == id);
            var trip = _mapper.Map<Trip, TripDTO>(await _unitOfWork.Trips.GetAsync(
                                        x => x.Include(y => y.AvailableSeats)
                                                                        .ThenInclude(x => x.Seat)
                                                                        .Include(x=>x.User)
                                                                        .Include(x=>x.Car)
                                                                        .ThenInclude(x=>x.Seats),
                                        x => x.Id == id));
            if (trip is null)
                throw new NotFoundException(nameof(TripDTO));

            trip.AvailableSeats.Select(x =>
            {
                if (usersBookedTrip.Any(y => y.SeatId == x.SeatId))
                {
                    x.AvailableSeatsType = AvailableSeatTypeDTO.Booked;
                }
                return x;
            }).ToList();

            trip.User.UserImg = _hostSettings.CurrentHost + trip.User.UserImg;
            trip.Car.CarDocuments = trip.Car.CarDocuments.Select(c =>
            {
                c.TechPassport = _hostSettings.CurrentHost + c.TechPassport;
                return c;
            }).ToList();
            
            return trip;

        }

        public async Task<IEnumerable<GetTripWithTripUsersDTO>> GetUserTripsAsync(ClaimsPrincipal principal)
        {
            string userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
            var trips = _mapper.Map<IEnumerable<TripDTO>>
                (await _unitOfWork.Trips.GetAsync(null, 
                    x=>x.Include(i=>i.Car)
                        .Include(x=>x.AvailableSeats)
                        .Include(i=>i.TripUsers).ThenInclude(i=>i.User)
                        .Include(i => i.TripUsers).ThenInclude(x=>x.Seat), 
                    x=>x.UserId == Guid.Parse((ReadOnlySpan<char>)userId)));

            if (!trips.Any()) throw new NotFoundException(nameof(UserDTO));

            trips = trips.Select(t =>
            {
                t.Car.CarDocuments = t.Car.CarDocuments.Select(c =>
                {
                    c.TechPassport = _hostSettings.CurrentHost + c.TechPassport;
                    return c;
                }).ToList();

                t.TripUsers = t.TripUsers.Select(tu =>
                {
                    
                    tu.User.UserImg = _hostSettings.CurrentHost + tu.User.UserImg;
                    return tu;
                }).ToList();
                return t;
            }).ToList();

            var listGroupByUsers = trips.First().TripUsers
                .GroupBy(x => x.UserId)
                .Select(x=>new GetBookedTripUsersDTO
                                                            { UserId= x.Key, 
                                                            User = x.Select(u=>u.User).FirstOrDefault() ,
                                                            Seats =  x.Select(x=>x.Seat).ToList()
                                                            }).ToList();
            var result = _mapper.Map<IEnumerable<GetTripWithTripUsersDTO>>(trips);
            result = result.Select(trip =>
            {
                trip.BookedTripUsers = listGroupByUsers.Select(l =>
                {
                    if (trip.TripUsers.Any(user=>user.UserId == l.UserId))
                    {
                        return l;
                    }
                    return null;
                }).ToList()!;
                return trip;
            }).ToList();
            
            return result;
        }

        public async Task<IEnumerable<TripDTO>> SearchTripsAsync(SearchTripDTO model)
        {
            var trip = await _unitOfWork.Trips.GetAsync(
                orderBy: null,
                includes: x => x.Include(x => x.AvailableSeats)
                    .Include(x => x.TripUsers)
                    .Include(x=>x.User),
                filter: x => x.StartPlace.Contains(model.StartPlace) 
                             && x.EndPlace.Contains(model.EndPlace) 
                             && x.StartTime.Date == model.StartTime.Date
                             && x.AvailableSeats.Count(s=>x.TripUsers.All(u=>u.SeatId != s.SeatId)) >= model.CountOfSeats,
                skip: model.Skip,
                take: model.Take);


            trip = trip.Select(t =>
            {
                t.User.UserImg = _hostSettings.CurrentHost + t.User.UserImg;
                return t;
            });

            var res = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripDTO>>(trip);
            return res;
        }

        public async Task<bool> AddTripAsync(CreateTripDTO newTripModel, ClaimsPrincipal principal)
        {
        
            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot create trip!");

            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            var tripModel = _mapper.Map<CreateTripDTO, TripDTO>(newTripModel);
            tripModel.UserId = userId;

            var trip = _mapper.Map<TripDTO, Trip>(tripModel);

            await _unitOfWork.Trips.InsertAsync(trip);
            return await _unitOfWork.SaveAsync(userId);
            
        }

        public async Task<bool> UpdateTripAsync(TripDTO tripModel)
        {
            //if (tripModel != null)
            //{
            //    var trip = _mapper.Map<TripModel, TripModel>(tripModel);
            //    _unitOfWork.Trips.Update(trip);
            //    return await _unitOfWork.SaveAsync();
            //}
            return false;
        }

        public async Task<bool> DeleteTripAsync(Guid id, ClaimsPrincipal principal)
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
                    NotificationStatus = NotificationDTOStatus.SpecificUser,
                    Text = $"The trip {trip.StartPlace} - {trip.EndPlace} was cancelled!"
                });
            });

            //if (tripUsers != null) _unitOfWork.TripUser.Delete(tripUsers);
            _unitOfWork.Trips.Delete(trip);

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }
    }
}
