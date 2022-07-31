using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services
{
    public class BookedTripsService: IBookedTripsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBookedSeatsService _bookedSeatsService;
        private readonly IUserService _userService;
        public BookedTripsService(IUnitOfWork unitOfWork, IMapper mapper, IBookedSeatsService bookedSeatsService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bookedSeatsService = bookedSeatsService;
            _userService = userService;
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

        public async Task<bool> AddBookedTripAsync(AddNewBookTrip tripModel, ClaimsPrincipal principal)
        {
            if (tripModel != null)
            {

                var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
                if (!checkIfUserExist) throw new Exception("This user cannot book trip!");

                var bookedTripModel = _mapper.Map<AddNewBookTrip, BookedTripModel>(tripModel);
                bookedTripModel.UserId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;

                var booked = await _bookedSeatsService.AddTripSeatsAsync(bookedTripModel, tripModel.CountOfSeats);


                var a = _mapper.Map<BookedTripModel, BookedTrip>(bookedTripModel);
                await _unitOfWork.BookedTrips.InsertAsync(a);

                return await _unitOfWork.SaveAsync();

                // var bookedTrip = _mapper.Map<AddNewBookTrip, BookedTrip>(tripModel);
                // bookedTrip.UserId = principal.Claims.FirstOrDefault(x=>x.Type == JwtClaimTypes.Id).Value;
                // bookedTrip.BookedSeats = new List<BookedSeat>()
                //     { new BookedSeat() { BookedTrip = bookedTrip, SeatId = 3 } };

                // await _unitOfWork.BookedTrips.InsertAsync(bookedTrip);


                // //var seat = new BookedSeat() { BookedTrip = bookedTrip, SeatId = 2 };
                // //await _unitOfWork.BookedSeats.InsertAsync(seat);

                // await _unitOfWork.SaveAsync();



                //var bookedTrips = await _unitOfWork.BookedTrips.GetAsync(null,
                //    x => x.Include(x => x.BookedSeats),
                //    x => x.TripId == tripModel.TripId);

                //var seats = await _unitOfWork.TripSeats.GetAsync(null, null, x => x.TripId == tripModel.TripId);
                //seats = seats.Where(x => bookedTrips.Any(y => y.BookedSeats.Any(z => z.SeatId != x.Id))).ToList();

                //// var a = seats.Where(x => bookedTrips.Any(y => y.BookedSeats.Any(z => z.SeatId != x.Id))).ToList();

                //List<BookedSeatModel> bookedSeats = new List<BookedSeatModel>();
                //for (int i = 0; i < tripModel.CountOfSeats; i++)
                //{
                //    var b = seats.ElementAt(i).Id;
                //    bookedSeats.Add(new BookedSeatModel() { BookedTripId = bookedTripModel.Id, SeatId = b });
                //}


                //var res = await _bookedSeatsService.AddTripSeatsAsync(tripModel);
                //if (!res) throw new Exception("Something went wrong with booked seats");


            }

            throw new Exception("Incorrect trip data!");
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
