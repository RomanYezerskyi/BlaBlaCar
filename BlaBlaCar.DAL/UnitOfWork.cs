using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private BaseRepositoryAsync<ApplicationUser> _users;
        private BaseRepositoryAsync<Trip> _trips;
        private BaseRepositoryAsync<Seat> _tripSeats;
        private BaseRepositoryAsync<AvailableSeats> _availableSeats;
        private BaseRepositoryAsync<Car> _car;
        private BaseRepositoryAsync<TripUser> _tripUser;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepositoryAsync<AvailableSeats> AvaliableSeats
        {
            get
            {
                return _availableSeats ??= new BaseRepositoryAsync<AvailableSeats>(_context);
            }
        }

        public IRepositoryAsync<Car> Cars
        {
            get
            {
                return _car ??= new BaseRepositoryAsync<Car>(_context);
            }
        }

        public IRepositoryAsync<TripUser> TripUser
        {
            get
            {
                return _tripUser ??= new BaseRepositoryAsync<TripUser>(_context);
            }
        }

        public IRepositoryAsync<ApplicationUser> Users
        {
            get
            {
                return _users ??= new BaseRepositoryAsync<ApplicationUser>(_context);
            }
        }

        public IRepositoryAsync<Trip> Trips
        {
            get
            {
                return _trips ??= new BaseRepositoryAsync<Trip>(_context);
            }
        }

        public IRepositoryAsync<Seat> TripSeats
        {
            get
            {
                return _tripSeats ??= new BaseRepositoryAsync<Seat>(_context);
            }
        }

       


        public async Task<bool> SaveAsync()
        {
            return Convert.ToBoolean(await _context.SaveChangesAsync());
        }
    }
}
