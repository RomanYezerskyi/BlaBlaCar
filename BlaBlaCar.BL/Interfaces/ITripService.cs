using System.Security.Claims;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripService
    {
        Task<TripDTO> GetTripAsync(Guid id);
        Task<UserTripsViewModel> GetUserTripsAsync(int take, int skip, ClaimsPrincipal principal);
        Task<SearchTripsResponseDTO> SearchTripsAsync(SearchTripDTO model);
        Task<bool> AddTripAsync(CreateTripDTO tripModel, ClaimsPrincipal principal);
        Task<bool> UpdateTripAsync(TripDTO tripModel);
        Task<bool> DeleteTripAsync(Guid id, ClaimsPrincipal principal);

    }
}
