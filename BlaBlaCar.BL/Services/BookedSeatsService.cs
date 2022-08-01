using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        //public async Task<BookedSeatModel> GetTripSeatAsync(int id)
        //{
        //    //var trip = _mapper.Map<BookedSeat, BookedSeatModel>
        //    //    (await _unitOfWork.BookedSeats.GetAsync(null, x => x.Id == id));
        //    //return trip;
        //    throw new NotImplementedException();
        //}

        //public async Task<IEnumerable<BookedSeatModel>> GetSeatsByTripIdAsync(int tripId)
        //{
        //    //var trips = _mapper.Map<IEnumerable<BookedSeat>, IEnumerable<BookedSeatModel>>
        //    //    (await _unitOfWork.BookedSeats.GetAsync(null, null, x => x.BookedTripId == tripId));
        //    //return trips;
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> AddTripSeatsAsync(IEnumerable<BookedSeatModel> tripModel)
        //{
        //    //var seats = _mapper.Map<IEnumerable<BookedSeatModel>, IEnumerable<BookedSeat>>(tripModel);
        //    //await _unitOfWork.BookedSeats.InsertRangeAsync(seats);
        //    //return await _unitOfWork.SaveAsync();
        //    throw new NotImplementedException();
        //}

        //public async Task<BookedTripModel> AddTripSeatsAsync(BookedTripModel tripModel, int requestedCount)
        //{

        //    //var bookedTrips = await _unitOfWork.BookedTrips.GetAsync(null,
        //    //    x => x.Include(x => x.BookedSeats),
        //    //    x => x.TripId == tripModel.TripId);

        //    //var seats = await _unitOfWork.TripSeats.GetAsync(null, null, x => x.TripId == tripModel.TripId);
        //    //seats = seats.Where(x => bookedTrips.All(y => y.BookedSeats.All(z => z.SeatId != x.Id))).ToList();

        //    //if (requestedCount > seats.ToList().Count()) throw new Exception("There are no available seats on this trip");

        //    //tripModel.BookedSeats = new List<BookedSeatModel>();
        //    //for (int i = 0; i < requestedCount; i++)
        //    //{
        //    //    tripModel.BookedSeats.Add(
        //    //        new BookedSeatModel() { BookedTrip = tripModel, SeatId = seats.ElementAt(i).Id }
        //    //    );
        //    //}

        //    //return tripModel;
        //    throw new NotImplementedException();
        //}

        //public Task<bool> UpdateTripSeatAsync(BookedSeatModel tripModel)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> DeleteTripSeatAsync(int id)
        //{
        //    //var tripSeat = await _unitOfWork.BookedSeats.GetAsync(includes: null, filter: x => x.Id == id);
        //    //if (tripSeat == null) throw new Exception("No information about the seat!");
        //    //_unitOfWork.BookedSeats.Delete(tripSeat);
        //    //return await _unitOfWork.SaveAsync();
        //    throw new NotImplementedException();
        //}
    }
}
