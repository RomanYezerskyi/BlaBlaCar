﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedTripsService
    {
        Task<TripUserModel> GetBookedTripAsync(Guid id);
        //Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync();
        //Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync(BookedTripModel model);
        Task<bool> AddBookedTripAsync(AddNewBookTrip tripModel, ClaimsPrincipal principal);
        //Task<bool> UpdateBookedTripAsync(BookedTripModel tripModel);
        Task<bool> DeleteBookedTripAsync(IEnumerable<TripUserModel> tripUserModel);
    }
}
