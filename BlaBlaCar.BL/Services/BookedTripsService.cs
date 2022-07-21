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
    public class BookedTripsService: IBookedTripsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBookedSeatsService _bookedSeatsService;
        public BookedTripsService(IUnitOfWork unitOfWork, IMapper mapper, IBookedSeatsService bookedSeatsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bookedSeatsService = bookedSeatsService;
        }
        public async Task<BookedTripModel> GetBookedTripAsync(int id)
        {
            var bookedTrip =
                _mapper.Map<BookedTrip, BookedTripModel>(await _unitOfWork.BookedTrips.GetAsync(null, x => x.Id == id));
            return bookedTrip;
        }

        public async Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync()
        {
            var bookedTrip =
                _mapper.Map<IEnumerable<BookedTrip>, IEnumerable<BookedTripModel>>
                    (await _unitOfWork.BookedTrips.GetAsync(null, null,null));
            return bookedTrip;
        }

        public Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync(BookedTripModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddBookedTripAsync(BookedTripModel tripModel)
        {
            if (tripModel != null)
            {
                var trip = _mapper.Map<BookedTripModel, BookedTrip>(tripModel);
                await _unitOfWork.BookedTrips.InsertAsync(trip);

                var res = await _bookedSeatsService.AddTripSeatsAsync(tripModel);
                if (!res) throw new Exception("Something went wrong with booked seats");
                return await _unitOfWork.SaveAsync();

            }

            return false;
        }

        public async Task<bool> UpdateBookedTripAsync(BookedTripModel tripModel)
        {
            if (tripModel != null)
            {
                var trip = _mapper.Map<BookedTripModel, BookedTrip>(tripModel);
                _unitOfWork.BookedTrips.Update(trip);


                return await _unitOfWork.SaveAsync();

            }

            return false;
        }

        public async Task<bool> DeleteBookedTripAsync(int id)
        {
            var trip = await _unitOfWork.BookedTrips.GetAsync(includes: null, filter: x => x.Id == id);
            if (trip == null) throw new Exception("No information about this trip! Trip cannot be deleted!");
            _unitOfWork.BookedTrips.Delete(trip);
            return await _unitOfWork.SaveAsync();
        }
    }
}
