using System.Security.Claims;
using BlaBlaCar.BL.DTOs.TripDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripService
    {
        Task<TripDTO> GetTripAsync(Guid id);
        Task<IEnumerable<GetTripWithTripUsersDTO>> GetUserTripsAsync(ClaimsPrincipal principal);
        Task<SearchTripsResponseDTO> SearchTripsAsync(SearchTripDTO model);
        Task<bool> AddTripAsync(CreateTripDTO tripModel, ClaimsPrincipal principal);
        Task<bool> UpdateTripAsync(TripDTO tripModel);
        Task<bool> DeleteTripAsync(Guid id, ClaimsPrincipal principal);

    }
}
