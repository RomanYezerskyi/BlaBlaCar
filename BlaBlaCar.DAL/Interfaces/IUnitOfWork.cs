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
        IRepositoryAsync<UserDocuments> UserDocuments { get; }
        IRepositoryAsync<CarDocuments> CarDocuments { get; }
        Task<bool> SaveAsync(Guid userId);
    }
}
