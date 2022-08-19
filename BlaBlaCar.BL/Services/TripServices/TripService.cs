using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Interfaces;
using BlaBlaCar.DAL.Entities;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly HostSettings _hostSettings;
       
        public TripService(IUnitOfWork unitOfWork,
            IMapper mapper,
             IUserService userService, IOptionsSnapshot<HostSettings> hostSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<TripModel> GetTripAsync(Guid id)
        {
            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == id);
            var trip = _mapper.Map<Trip, TripModel>(await _unitOfWork.Trips.GetAsync(
                                        x => x.Include(y => y.AvailableSeats)
                                                                        .ThenInclude(x => x.Seat)
                                                                        .Include(x=>x.User)
                                                                        .Include(x=>x.Car)
                                                                        .ThenInclude(x=>x.Seats),
                                        x => x.Id == id));
            trip.AvailableSeats.Select(x =>
            {
                if (usersBookedTrip.Any(y => y.SeatId == x.SeatId))
                {
                    x.AvailableSeatsType = AvailableSeatsType.Booked;
                }
                return x;
            }).ToList();

            trip.User.UserImg = _hostSettings.Host + trip.User.UserImg;
            trip.Car.CarDocuments = trip.Car.CarDocuments.Select(c =>
            {
                c.TechPassport = _hostSettings.Host + c.TechPassport;
                return c;
            }).ToList();
            
            return trip;

        }

        public async Task<IEnumerable<TripAndTripUsersViewModel>> GetUserTripsAsync(ClaimsPrincipal principal)
        {
            string userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;
            var trips = _mapper.Map<IEnumerable<TripModel>>
                (await _unitOfWork.Trips.GetAsync(null, 
                    x=>x.Include(i=>i.Car)
                        .Include(x=>x.AvailableSeats)
                        .Include(i=>i.TripUsers).ThenInclude(i=>i.User)
                        .Include(i => i.TripUsers).ThenInclude(x=>x.Seat), 
                    x=>x.UserId == Guid.Parse((ReadOnlySpan<char>)userId)));
            if (!trips.Any()) return null;

            trips = trips.Select(t =>
            {
                t.Car.CarDocuments = t.Car.CarDocuments.Select(c =>
                {
                    c.TechPassport = _hostSettings.Host + c.TechPassport;
                    return c;
                }).ToList();

                t.TripUsers = t.TripUsers.Select(tu =>
                {
                    
                    tu.User.UserImg = _hostSettings.Host + tu.User.UserImg;
                    return tu;
                }).ToList();
                return t;
            }).ToList();

            var listGroupByUsers = trips.First().TripUsers
                .GroupBy(x => x.UserId)
                .Select(x=>new BookedTripUsersViewModel
                                                            { UserId= x.Key, 
                                                            User = x.Select(u=>u.User).FirstOrDefault() ,
                                                            Seats =  x.Select(x=>x.Seat).ToList()
                                                            }).ToList();
            var result = _mapper.Map<IEnumerable<TripAndTripUsersViewModel>>(trips);
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

        public async Task<IEnumerable<TripModel>> SearchTripsAsync(SearchTripModel model)
        {

            var trip = await _unitOfWork.Trips.GetAsync(null,
                x => x.Include(x => x.AvailableSeats)
                                                .Include(x => x.TripUsers)
                                                .Include(x=>x.User),
                x => x.StartPlace.Contains(model.StartPlace) && x.EndPlace.Contains(model.EndPlace) && x.StartTime >= model.StartTime);
            trip = trip.Select(t =>
            {
                t.User.UserImg = _hostSettings.Host + t.User.UserImg;
                return t;
            });

            var res = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripModel>>(trip);
            return res;
        }

        public async Task<bool> AddTripAsync(NewTripViewModel newTripModel, ClaimsPrincipal principal)
        {
            if (newTripModel != null)
            {

                var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
                if (!checkIfUserExist) throw new Exception("This user cannot create trip!");

                var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

                var tripModel = _mapper.Map<NewTripViewModel, TripModel>(newTripModel);
                tripModel.UserId = userId;

                var trip = _mapper.Map<TripModel, Trip>(tripModel);

                await _unitOfWork.Trips.InsertAsync(trip);
                return await _unitOfWork.SaveAsync(userId);
            }

            throw new Exception("Trip cannot be added because not all data is available");
        }

        public async Task<bool> UpdateTripAsync(TripModel tripModel)
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
            var trip = await _unitOfWork.Trips.GetAsync(includes: null, filter: x => x.Id == id);
            if (trip == null) throw new Exception("No information about this trip! Trip cannot be deleted!");
            var availableSeats = await _unitOfWork.AvaliableSeats.GetAsync(null, null, x => x.TripId == id);
            if(availableSeats != null) _unitOfWork.AvaliableSeats.Delete(availableSeats);
            var tripUsers = await _unitOfWork.TripUser.GetAsync(null, null, x => x.TripId == id);
            if (tripUsers != null) _unitOfWork.TripUser.Delete(tripUsers);
            _unitOfWork.Trips.Delete(trip);

            var changedBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(changedBy);
        }
    }
}
