using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripSeatsService
    {
        Task<SeatModel> GetTripSeatAsync(int id);
        Task<IEnumerable<SeatModel>> GetSeatsByTripIdAsync(int tripId);
        Task<bool> AddTripSeatsAsync(IEnumerable<SeatModel> tripModel);
        Task<TripModel> AddTripSeatsAsync(TripModel trip, int count);
        Task<bool> UpdateTripSeatAsync(SeatModel tripModel);
        Task<bool> DeleteTripSeatAsync(int id);
    }
}
