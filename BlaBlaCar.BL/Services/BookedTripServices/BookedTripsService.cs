using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;

namespace BlaBlaCar.BL.Services.BookedTripServices
{
    public class BookedTripsService : IBookedTripsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        public BookedTripsService(IUnitOfWork unitOfWork, IMapper mapper,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<bool> DeleteBookedTripAsync(IEnumerable<TripUserModel> tripUserModel)
        {
            var trip = _mapper.Map<IEnumerable<TripUserModel>>(await _unitOfWork.TripUser.GetAsync(null,null, x => tripUserModel.Any(y=>y.Id == x.Id)));
            if (trip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));
            return await _unitOfWork.SaveAsync();
        }
    }
}
