using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryAsync<User> Users { get; }
        IRepositoryAsync<Trip> Trips { get; }
        IRepositoryAsync<UserTrip> UserTrips { get; }
        IRepositoryAsync<Seat> TripSeats { get; }
        IRepositoryAsync<BookedSeat> BookedSeats { get; }
        IRepositoryAsync<BookedTrip> BookedTrips { get; }
        Task<bool> SaveAsync();
    }
}
