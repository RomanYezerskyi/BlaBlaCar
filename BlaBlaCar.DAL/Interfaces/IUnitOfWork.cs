using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryAsync<ApplicationUser> Users { get; }
        IRepositoryAsync<Trip> Trips { get; }
        IRepositoryAsync<Seat> TripSeats { get; }
        IRepositoryAsync<AvailableSeats> AvaliableSeats { get; }
        IRepositoryAsync<Car> Cars { get; }
        IRepositoryAsync<TripUser> TripUser { get; }
        Task<bool> SaveAsync();
    }
}
