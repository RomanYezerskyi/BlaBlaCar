using System.Security.Claims;
using BlaBlaCar.BL.DTOs.TripDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ITripService
    {
        Task<TripDTO> GetTripAsync(Guid id, Guid currentUserId);
        Task<UserTripsDTO> GetUserTripsAsync(int take, int skip, Guid currentUserId);
        Task<UserTripsDTO> GetUserStartedTripsAsync(int take, int skip, Guid currentUserId);
        Task<SearchTripsResponseDTO> SearchTripsAsync(SearchTripDTO model);
        Task<bool> AddTripAsync(CreateTripDTO tripModel, Guid currentUserId);
        Task<bool> DeleteTripAsync(Guid id, Guid currentUserId);

    }
}
