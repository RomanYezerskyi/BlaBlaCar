using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Models;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripSeatsService
    {
        Task<SeatModel> GetTripSeatAsync(int id);
        Task<IEnumerable<SeatModel>> GetSeatsByTripIdAsync(int tripId);
        Task<bool> AddTripSeatsAsync(IEnumerable<SeatModel> tripModel);
        Task<bool> AddTripSeatsAsync(int tripId, int count);
        Task<bool> UpdateTripSeatAsync(SeatModel tripModel);
        Task<bool> DeleteTripSeatAsync(int id);
    }
}
