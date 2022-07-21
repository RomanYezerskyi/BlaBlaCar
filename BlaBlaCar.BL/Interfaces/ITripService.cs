using BlaBlaCar.BL.Models;


namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripService
    {
        Task<TripModel> GetTripAsync(int id);
        Task<IEnumerable<TripModel>> GetTripsAsync();
        Task<IEnumerable<TripModel>> GetTripsAsync(TripModel model);
        Task<bool> AddTripAsync(TripModel tripModel);
        Task<bool> UpdateTripAsync(TripModel tripModel);
        Task<bool> DeleteTripAsync(int id);

    }
}
