using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Models;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedTripsService
    {
        Task<BookedTripModel> GetBookedTripAsync(int id);
        Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync();
        Task<IEnumerable<BookedTripModel>> GetBookedTripsAsync(BookedTripModel model);
        Task<bool> AddBookedTripAsync(BookedTripModel tripModel);
        Task<bool> UpdateBookedTripAsync(BookedTripModel tripModel);
        Task<bool> DeleteBookedTripAsync(int id);
    }
}
