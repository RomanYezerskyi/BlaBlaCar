using System.Security.Claims;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripService
    {
        Task<TripModel> GetTripAsync(Guid id);
        Task<IEnumerable<TripModel>> GetTripsAsync();
        Task<IEnumerable<TripModel>> SearchTripsAsync(SearchTripModel model);
        Task<bool> AddTripAsync(NewTripViewModel tripModel, ClaimsPrincipal principal);
        Task<bool> UpdateTripAsync(TripModel tripModel);
        Task<bool> DeleteTripAsync(Guid id);

    }
}
