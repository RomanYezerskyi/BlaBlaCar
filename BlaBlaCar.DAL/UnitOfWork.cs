using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private BaseRepositoryAsync<User> _users;
        private BaseRepositoryAsync<Trip> _trips;
        private BaseRepositoryAsync<UserTrip> _userTrips;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepositoryAsync<User> Users
        {
            get
            {
                return _users ??= new BaseRepositoryAsync<User>(_context);
            }
        }

        public IRepositoryAsync<Trip> Trips
        {
            get
            {
                return _trips ??= new BaseRepositoryAsync<Trip>(_context);
            }
        }

        public IRepositoryAsync<UserTrip> UserTrips
        {
            get
            {
                return _userTrips ??= new BaseRepositoryAsync<UserTrip>(_context);
            }
        }


        public async Task<bool> SaveAsync()
        {
            return Convert.ToBoolean(await _context.SaveChangesAsync());
        }
    }
}
