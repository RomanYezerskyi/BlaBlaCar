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
      
        public TripService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<TripModel> GetTripAsync(int id)
        {
            var trip = _mapper.Map<Trip, TripModel>(await _unitOfWork.Trips.GetAsync(null, x=>x.Id == id));
            return trip;
        }

        public async Task<IEnumerable<TripModel>> GetTripAsync()
        {
            var trip = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripModel>>
                (await _unitOfWork.Trips.GetAsync(null, null, null));
            return trip;
        }

        public async Task<bool> AddTripAsync(TripModel tripModel)
        {
            if (tripModel != null)
            {
                var trip = _mapper.Map<TripModel, Trip>(tripModel);
                await _unitOfWork.Trips.InsertAsync(trip);
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
            if (trip == null) throw new Exception("No information about this film! Film cannot be deleted!");
            _unitOfWork.Trips.Delete(trip);
            return await _unitOfWork.SaveAsync();
        }
    }
}
