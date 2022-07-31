using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
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
        private readonly ITripSeatsService _tripSeatsService;
        private readonly IUserService _userService;
        
        public TripService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            ITripSeatsService tripSeatsService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tripSeatsService = tripSeatsService;
            _userService = userService;
        }
        public async Task<TripModel> GetTripAsync(int id)
        {
            var trip = _mapper.Map<Trip, TripModel>(await _unitOfWork.Trips.GetAsync(null, x=>x.Id == id));
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

            var trip = await _unitOfWork.Trips.GetAsync(null, x=>x.Include(x=>x.BookedTrips).ThenInclude(x=>x.BookedSeats),
                x => x.StartPlace.Contains(model.StartPlace) && 
                     x.EndPlace.Contains(model.EndPlace)
                     && x.StartTime.Date.Equals(model.StartTime) && x.Seats.Count >= model.CountOfSeats);




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
                tripModel.UserId = principal.Claims.FirstOrDefault(x=>x.Type == JwtClaimTypes.Id).Value;
                
                var res = await _tripSeatsService.AddTripSeatsAsync(tripModel, tripModel.CountOfSeats);

                var trip = _mapper.Map<TripModel, Trip>(tripModel);
                await _unitOfWork.Trips.InsertAsync(trip);
                return await _unitOfWork.SaveAsync();
            }

            throw new Exception("Trip cannot be added because not all data is available");
        }

        public async Task<bool> UpdateTripAsync(TripModel tripModel)
        {
            if (tripModel != null)
            {
                var trip = _mapper.Map<TripModel, Trip>(tripModel);
                _unitOfWork.Trips.Update(trip);
                return await _unitOfWork.SaveAsync();
            }
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
