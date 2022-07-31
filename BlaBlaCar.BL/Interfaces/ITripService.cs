using System.Security.Claims;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripService
    {
        Task<TripModel> GetTripAsync(int id);
        Task<IEnumerable<TripModel>> GetTripsAsync();
        Task<IEnumerable<TripModel>> SearchTripsAsync(SearchTripModel model);
        Task<bool> AddTripAsync(AddNewTripModel tripModel, ClaimsPrincipal principal);
        Task<bool> UpdateTripAsync(TripModel tripModel);
        Task<bool> DeleteTripAsync(int id);

    }
}
