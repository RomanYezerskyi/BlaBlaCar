using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Models;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.BL.Services
{
    public class TripSeatsService: ITripSeatsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TripSeatsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<SeatModel> GetTripSeatAsync(int id)
        {
            var trip = _mapper.Map<Seat, SeatModel>
                (await _unitOfWork.TripSeats.GetAsync(null, x => x.Id == id));
            return trip;
        }

        public async Task<IEnumerable<SeatModel>> GetSeatsByTripIdAsync(int tripId)
        {
            var trips = _mapper.Map<IEnumerable<Seat>, IEnumerable<SeatModel>>
                (await _unitOfWork.TripSeats.GetAsync(null,null, x => x.TripId == tripId));
            return trips;
        }

        public async Task<bool> AddTripSeatsAsync(IEnumerable<SeatModel> tripModel)
        {
            var seats = _mapper.Map<IEnumerable<SeatModel>, IEnumerable<Seat>>(tripModel);
            await _unitOfWork.TripSeats.InsertRangeAsync(seats);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> AddTripSeatsAsync(int tripId, int count)
        {
            List<SeatModel> seatModels = new List<SeatModel>();
            for (int i = 0; i < count; i++)
            {
                seatModels.Add(new SeatModel { TripId = tripId, Num = i + 1 });
            }
            await _unitOfWork.TripSeats.InsertRangeAsync(_mapper.Map<IEnumerable<Seat>>(seatModels));
            return await _unitOfWork.SaveAsync();
        }

        public Task<bool> UpdateTripSeatAsync(SeatModel tripModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTripSeatAsync(int id)
        {
            var tripSeat = await _unitOfWork.TripSeats.GetAsync(includes: null, filter: x => x.Id == id);
            if (tripSeat == null) throw new Exception("No information about the seat!");
            _unitOfWork.TripSeats.Delete(tripSeat);
            return await _unitOfWork.SaveAsync();
        }
    }
}
