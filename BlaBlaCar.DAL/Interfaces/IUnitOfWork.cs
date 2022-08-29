using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Entities.TripEntities;

namespace BlaBlaCar.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryAsync<ApplicationUser> Users { get; }
        IRepositoryAsync<Trip> Trips { get; }
        IRepositoryAsync<Seat> CarSeats { get; }
        IRepositoryAsync<AvailableSeats> AvailableSeats { get; }
        IRepositoryAsync<Car> Cars { get; }
        IRepositoryAsync<TripUser> TripUser { get; }
        IRepositoryAsync<UserDocuments> UserDocuments { get; }
        IRepositoryAsync<CarDocuments> CarDocuments { get; }
        IRepositoryAsync<Notifications> Notifications { get; }
        IRepositoryAsync<ReadNotifications> ReadNotifications { get; }
        Task<bool> SaveAsync(Guid userId);
    }
}
