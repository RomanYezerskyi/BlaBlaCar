using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.BookTripModels;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.ViewModels.AdminViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedTripsService
    {
        Task<UserBookedTripsViewModel> GetUserBookedTripsAsync(int take, int skip, ClaimsPrincipal claimsPrincipal);
        Task<bool> AddBookedTripAsync(AddNewBookTripDTO tripModel, ClaimsPrincipal principal);
        Task<bool> DeleteBookedTripAsync(DeleteTripUserDTO tripUserModel, ClaimsPrincipal principal);
        Task<bool> DeleteBookedSeatAsync(UpdateTripUserDTO tripUserModel, ClaimsPrincipal principal);
        Task<bool> DeleteUserFromTripAsync(UpdateTripUserDTO tripUserModel, ClaimsPrincipal principal);
    }
}
