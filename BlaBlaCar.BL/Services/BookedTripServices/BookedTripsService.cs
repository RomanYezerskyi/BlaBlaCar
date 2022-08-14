﻿using System.Security.Claims;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<TripModel>> GetUserTripsAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value;

            var trip = _mapper.Map<IEnumerable<TripModel>>(await _unitOfWork.Trips.GetAsync(x => x.OrderByDescending(x => x.StartTime), x =>
                    x.Include(x => x.TripUsers.Where(x => x.userId == userId)).ThenInclude(x => x.Seat),
                x => x.TripUsers
                    .Any(x => x.userId == userId)));
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

        public async Task<bool> DeleteBookedTripAsync(IEnumerable<TripUserViewModel> tripUserModel)
        {
            var trip = _mapper.Map<IEnumerable<TripUserModel>>(await _unitOfWork.TripUser.GetAsync(null, null, x =>
                x.TripId == tripUserModel.FirstOrDefault().TripId && x.userId == tripUserModel.FirstOrDefault().UserId));
            if (trip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteBookedSeatAsync(TripUserViewModel tripUserModel)
        {
            var trip = _mapper.Map<TripUserModel>(await _unitOfWork.TripUser.GetAsync(null, x =>
                x.TripId == tripUserModel.TripId && x.userId == tripUserModel.UserId && x.SeatId == tripUserModel.SeatId));
            if (trip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<TripUser>(trip));
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteUserFromTripAsync(TripUserViewModel tripUserModel)
        {
            var trip = _mapper.Map<IEnumerable<TripUserModel>>(await _unitOfWork.TripUser.GetAsync(null,null, x =>
                x.TripId == tripUserModel.TripId && x.userId == tripUserModel.UserId && x.TripId == tripUserModel.TripId));
            if (trip == null) throw new Exception("Trip not found");
            _unitOfWork.TripUser.Delete(_mapper.Map<IEnumerable<TripUser>>(trip));
            return await _unitOfWork.SaveAsync();
        }
    }
    
}
