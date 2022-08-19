using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedTripsService
    {
        Task<IEnumerable<TripModel>> GetUserBookedTripsAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> AddBookedTripAsync(AddNewBookTrip tripModel, ClaimsPrincipal principal);
        Task<bool> DeleteBookedTripAsync(IEnumerable<TripUserViewModel> tripUserModel, ClaimsPrincipal principal);
        Task<bool> DeleteBookedSeatAsync(TripUserViewModel tripUserModel, ClaimsPrincipal principal);
        Task<bool> DeleteUserFromTripAsync(TripUserViewModel tripUserModel, ClaimsPrincipal principal);
    }
}
