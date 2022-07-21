using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Models;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.BL.Services
{
    public class TripService: ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserTripsService _userTripsService;
        private readonly ITripSeatsService _tripSeatsService;
      
        public TripService(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IUserTripsService userTripsService, 
            ITripSeatsService tripSeatsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userTripsService = userTripsService;
            _tripSeatsService = tripSeatsService;
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

        public async Task<IEnumerable<TripModel>> GetTripsAsync(TripModel model)
        {
            var trip = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripModel>>
                (await _unitOfWork.Trips.GetAsync(null, null, 
                    x=>x.StartPlace.Contains(model.StartPlace) &&
                       x.EndPlace.Contains(model.EndPlace) && 
                       x.StartTime.Equals(model.StartTime)));
            return trip;
        }

        public async Task<bool> AddTripAsync(TripModel tripModel)
        {
            if (tripModel != null)
            {
                var trip = _mapper.Map<TripModel, Trip>(tripModel);
                await _unitOfWork.Trips.InsertAsync(trip);

                var userTripModel = new UserTripModel()
                {
                    TripId = trip.Id,
                    UserId = tripModel.UserId,
                };
                await _unitOfWork.UserTrips.InsertAsync(_mapper.Map<UserTrip>(userTripModel));

                //List<SeatModel> seatModels = new List<SeatModel>();
                //for (int i = 0; i < tripModel.CountOfSeats; i++)
                //{
                //    seatModels.Add(new SeatModel{ TripId = trip.Id});
                //}
                var res = await _tripSeatsService.AddTripSeatsAsync(trip.Id, tripModel.CountOfSeats);

                if (!res) throw new Exception("Something went wrong with adding seats");
                //await _tripSeatsService.AddTripSeatsAsync(seatModels);

               // await _unitOfWork.TripSeats.InsertRangeAsync(_mapper.Map<IEnumerable<Seat>>(seatModels));
                return await _unitOfWork.SaveAsync();

            }

            return false;
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
