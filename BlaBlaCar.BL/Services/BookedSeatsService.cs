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
    public class BookedSeatsService : IBookedSeatsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookedSeatsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BookedSeatModel> GetTripSeatAsync(int id)
        {
            var trip = _mapper.Map<BookedSeat, BookedSeatModel>
                (await _unitOfWork.BookedSeats.GetAsync(null, x => x.Id == id));
            return trip;
        }

        public async Task<IEnumerable<BookedSeatModel>> GetSeatsByTripIdAsync(int tripId)
        {
            var trips = _mapper.Map<IEnumerable<BookedSeat>, IEnumerable<BookedSeatModel>>
                (await _unitOfWork.BookedSeats.GetAsync(null, null, x => x.BookedTripId == tripId));
            return trips;
        }

        public async Task<bool> AddTripSeatsAsync(IEnumerable<BookedSeatModel> tripModel)
        {
            var seats = _mapper.Map<IEnumerable<BookedSeatModel>, IEnumerable<BookedSeat>>(tripModel);
            await _unitOfWork.BookedSeats.InsertRangeAsync(seats);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> AddTripSeatsAsync(BookedTripModel tripModel)
        {
            List<BookedSeatModel> seatModels = new List<BookedSeatModel>();
            foreach (var bookedSeat in tripModel.BookedSeats)
            {
                seatModels.Add(new BookedSeatModel{ BookedTripId = tripModel.TripId, SeatId = bookedSeat.SeatId});
            }
            await _unitOfWork.BookedSeats.InsertRangeAsync(_mapper.Map<IEnumerable<BookedSeat>>(seatModels));
            return await _unitOfWork.SaveAsync();
        }

        public Task<bool> UpdateTripSeatAsync(BookedSeatModel tripModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTripSeatAsync(int id)
        {
            var tripSeat = await _unitOfWork.BookedSeats.GetAsync(includes: null, filter: x => x.Id == id);
            if (tripSeat == null) throw new Exception("No information about the seat!");
            _unitOfWork.BookedSeats.Delete(tripSeat);
            return await _unitOfWork.SaveAsync();
        }
    }
}
