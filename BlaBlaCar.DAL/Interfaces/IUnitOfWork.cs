using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryAsync<User> Users { get; }
        IRepositoryAsync<Trip> Trips { get; }
        IRepositoryAsync<UserTrip> UserTrips { get; }
        Task<bool> SaveAsync();
    }
}
