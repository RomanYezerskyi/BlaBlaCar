using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlaBlaCar.BL.Services
{
    public class TripService: ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICarSeatsService _carSeatsService;
        public TripService(IUnitOfWork unitOfWork, 
            IMapper mapper,
             IUserService userService, ICarSeatsService carSeatsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _carSeatsService = carSeatsService;
        }
        public async Task<TripModel> GetTripAsync(int id)
        {
            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null,null,x => x.TripId == id);
            var trip = _mapper.Map<Trip, TripModel>(await _unitOfWork.Trips.GetAsync(
                                        x=>x.Include(
                                            y=>y.AvailableSeats).ThenInclude(x=>x.Seat), 
                                        x => x.Id == id));
            trip.AvailableSeats.Select(x => {
                    if (usersBookedTrip.Any(y => y.SeatId == x.SeatId))
                    {
                        x.AvailableSeatsType = AvailableSeatsType.Booked;
                    }
                    return x;
                }).ToList();
                

            var car = await _unitOfWork.Cars.GetAsync(null, x => x.Id == trip.CarId);
            trip.Car = _mapper.Map<CarModel>(car);
            return trip;

        }

        public async Task<IEnumerable<TripModel>> GetTripsAsync()
        {
            var trips = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripModel>>
                (await _unitOfWork.Trips.GetAsync(null, null, null));
            return trips;
        }

        public async Task<IEnumerable<TripModel>> SearchTripsAsync(SearchTripModel model)
        {

            var trip = await _unitOfWork.Trips.GetAsync(null,
                x => x.Include(x => x.AvailableSeats).Include(x => x.TripUsers),
                x => x.StartPlace.Contains(model.StartPlace) && x.StartTime <= model.StartTime);


            var res = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripModel>>(trip);
            return res;
        }

        public async Task<bool> AddTripAsync(AddNewTripModel newTripModel, ClaimsPrincipal principal)
        {
            if (newTripModel != null)
            {

                var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
                if (!checkIfUserExist) throw new Exception("This user cannot create trip!");

                var tripModel = _mapper.Map<AddNewTripModel, TripModel>(newTripModel);
                tripModel.UserId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;

                tripModel.CarId = newTripModel.CarId;

                await _carSeatsService.AddAvailableSeatsAsync(tripModel, newTripModel.CountOfSeats);

                var trip = _mapper.Map<TripModel, Trip>(tripModel);

                await _unitOfWork.Trips.InsertAsync(trip);
                return await _unitOfWork.SaveAsync();
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

        public async Task<bool> DeleteTripAsync(int id)
        {
            var trip = await _unitOfWork.Trips.GetAsync(includes: null, filter: x=>x.Id == id);
            if (trip == null) throw new Exception("No information about this trip! Trip cannot be deleted!");
            _unitOfWork.Trips.Delete(trip);
            return await _unitOfWork.SaveAsync();
        }
    }
}
