using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.BookTripModels;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedTripsService
    {
        Task<UserBookedTripsDTO> GetUserBookedTripsAsync(int take, int skip, Guid currentUserId);
        Task<UserBookedTripsDTO> GetUserStartedTripAsync(int take, int skip, Guid currentUserId);
        Task<bool> AddBookedTripAsync(NewBookTripModel tripModel, UserDTO currentUser);
        Task<bool> DeleteBookedTripAsync(DeleteTripUserDTO tripUserModel, Guid currentUserId, string userName);
        Task<bool> DeleteBookedSeatAsync(UpdateTripUserDTO tripUserModel, Guid currentUserId, string userName);
        Task<bool> DeleteUserFromTripAsync(UpdateTripUserDTO tripUserModel, Guid currentUserId);
    }
}
