using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.BookTripModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedTripsService
    {
        Task<BookedTripModel> GetBookedTripAsync(int id);
        Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync();
        Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync(BookedTripModel model);
        Task<bool> AddBookedTripAsync(AddNewBookTrip tripModel, ClaimsPrincipal principal);
        Task<bool> UpdateBookedTripAsync(BookedTripModel tripModel);
        Task<bool> DeleteBookedTripAsync(int id);
    }
}
