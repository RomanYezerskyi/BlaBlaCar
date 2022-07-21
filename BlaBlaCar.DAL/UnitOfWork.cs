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
        private BaseRepositoryAsync<Seat> _tripSeats;
        private BaseRepositoryAsync<BookedSeat> _bookedSeats;
        private BaseRepositoryAsync<BookedTrip> _bookedTrips;
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

        public IRepositoryAsync<Seat> TripSeats
        {
            get
            {
                return _tripSeats ??= new BaseRepositoryAsync<Seat>(_context);
            }
        }

        public IRepositoryAsync<BookedSeat> BookedSeats
        {
            get
            {
                return _bookedSeats ??= new BaseRepositoryAsync<BookedSeat>(_context);
            }
        }

        public IRepositoryAsync<BookedTrip> BookedTrips
        {
            get
            {
                return _bookedTrips ??= new BaseRepositoryAsync<BookedTrip>(_context);
            }
        }


        public async Task<bool> SaveAsync()
        {
            return Convert.ToBoolean(await _context.SaveChangesAsync());
        }
    }
}
