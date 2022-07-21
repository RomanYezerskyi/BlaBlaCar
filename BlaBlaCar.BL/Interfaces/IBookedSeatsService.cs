using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Models;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IBookedSeatsService
    {
        Task<BookedSeatModel> GetTripSeatAsync(int id);
        Task<IEnumerable<BookedSeatModel>> GetSeatsByTripIdAsync(int tripId);
        Task<bool> AddTripSeatsAsync(IEnumerable<BookedSeatModel> tripModel);
        Task<bool> AddTripSeatsAsync(BookedTripModel tripModel);
        Task<bool> UpdateTripSeatAsync(BookedSeatModel tripModel);
        Task<bool> DeleteTripSeatAsync(int id);
    }
}
