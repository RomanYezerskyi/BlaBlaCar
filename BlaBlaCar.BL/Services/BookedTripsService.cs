using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;
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

        public async Task<TripUserModel> GetBookedTripAsync(Guid id)
        {
            var trip = _mapper.Map<TripUser, TripUserModel>(await _unitOfWork.TripUser.GetAsync(null, x => x.Id == id));
            return trip;
        }

        public async Task<bool> AddBookedTripAsync(AddNewBookTrip tripModel, ClaimsPrincipal principal)
        {

            var checkIfUserExist = await _userService.СheckIfUserExistsAsync(principal);
            if (!checkIfUserExist) throw new Exception("This user cannot book trip!");
            string userId = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;

            var usersBookedTrip = await _unitOfWork.TripUser
                .GetAsync(null, null, x => x.TripId == tripModel.TripId);
            var checkIfSeatNotBooked = tripModel.BookedSeats.Any(x => usersBookedTrip.Any(y => y.SeatId == x.Id));
            if (checkIfSeatNotBooked) throw new Exception("This seats already booked!");

            var listOfSeats = new List<TripUserModel>();
            for (int i = 0; i < tripModel.RequestedSeats; i++)
            {
                var userTripModel = _mapper.Map<AddNewBookTrip, TripUserModel>(tripModel);

                userTripModel.UserId = userId;
                userTripModel.SeatId = tripModel.BookedSeats.ElementAt(i).Id;
                listOfSeats.Add(userTripModel);
            }
            await _unitOfWork.TripUser.InsertRangeAsync(_mapper.Map<IEnumerable<TripUser>>(listOfSeats));
            return await _unitOfWork.SaveAsync();
        }


        //public async Task<BookedTripModel> GetBookedTripAsync(int id)

        //{

        //    //var bookedTrip =

        //    //    _mapper.Map<BookedTrip, BookedTripModel>(await _unitOfWork.BookedTrips.GetAsync(null, x => x.Id == id));

        //    //return bookedTrip;

        //    throw new NotImplementedException();

        //}


        //public async Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync()

        //{

        //    //var bookedTrip =

        //    //    _mapper.Map<IEnumerable<BookedTrip>, IEnumerable<BookedTripModel>>

        //    //        (await _unitOfWork.BookedTrips.GetAsync(null, null,null));

        //    //return bookedTrip;

        //    throw new NotImplementedException();

        //}


        //public Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync(BookedTripModel model)

        //{

        //    throw new NotImplementedException();

        //}


        //        //var res = await _bookedSeatsService.AddTripSeatsAsync(tripModel);

        //        //if (!res) throw new Exception("Something went wrong with booked seats");


        //    }


        //    throw new Exception("Incorrect trip data!");

        //}


        //public async Task<bool> UpdateBookedTripAsync(BookedTripModel tripModel)

        //{

        //    //if (tripModel != null)

        //    //{

        //    //    var trip = _mapper.Map<BookedTripModel, BookedTrip>(tripModel);

        //    //    _unitOfWork.BookedTrips.Update(trip);


        //    //    return await _unitOfWork.SaveAsync();


        //    //}


        //    return false;

        //}


        //public async Task<bool> DeleteBookedTripAsync(int id)

        //{

        //    //var trip = await _unitOfWork.BookedTrips.GetAsync(includes: null, filter: x => x.Id == id);

        //    //if (trip == null) throw new Exception("No information about this trip! Trip cannot be deleted!");

        //    //_unitOfWork.BookedTrips.Delete(trip);

        //    //return await _unitOfWork.SaveAsync();

        //    throw new NotImplementedException();

        //}
    }
}
