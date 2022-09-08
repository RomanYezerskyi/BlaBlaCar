using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Entities.ChatEntities;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Entities.TripEntities;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private BaseRepositoryAsync<ApplicationUser> _users;
        private BaseRepositoryAsync<Trip> _trips;
        private BaseRepositoryAsync<Seat> _carSeats;
        private BaseRepositoryAsync<AvailableSeats> _availableSeats;
        private BaseRepositoryAsync<Car> _car;
        private BaseRepositoryAsync<TripUser> _tripUser;
        private BaseRepositoryAsync<UserDocuments> _userDocuments;
        private BaseRepositoryAsync<CarDocuments> _carDocuments;
        private BaseRepositoryAsync<Notifications> _notifications;
        private BaseRepositoryAsync<ReadNotifications> _readNotifications;
        private BaseRepositoryAsync<Chat> _chats;
        private BaseRepositoryAsync<Message> _messages;
        private BaseRepositoryAsync<UsersInChats> _usersInChats;
        private BaseRepositoryAsync<ReadMessages> _readMessages;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public  IRepositoryAsync<AvailableSeats> AvailableSeats
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

        public IRepositoryAsync<UserDocuments> UserDocuments
        {
            get
            {
                return _userDocuments ??= new BaseRepositoryAsync<UserDocuments>(_context);
            }
        }

        public IRepositoryAsync<CarDocuments> CarDocuments
        {
            get
            {
                return _carDocuments ??= new BaseRepositoryAsync<CarDocuments>(_context);
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

        public IRepositoryAsync<Seat> CarSeats
        {
            get
            {
                return _carSeats ??= new BaseRepositoryAsync<Seat>(_context);
            }
        }


        public IRepositoryAsync<Notifications> Notifications
        {
            get
            {
                return _notifications ??= new BaseRepositoryAsync<Notifications>(_context);
            }
        }

        public IRepositoryAsync<ReadNotifications> ReadNotifications
        {
            get
            {
                return _readNotifications ??= new BaseRepositoryAsync<ReadNotifications>(_context);
            }
        }

        public IRepositoryAsync<Chat> Chats
        {
            get
            {
                return _chats ??= new BaseRepositoryAsync<Chat>(_context);
            }
        }

        public IRepositoryAsync<Message> Messages
        {
            get
            {
                return _messages ??= new BaseRepositoryAsync<Message>(_context);
            }
        }

        public IRepositoryAsync<UsersInChats> UsersInChats
        {
            get
            {
                return _usersInChats ??= new BaseRepositoryAsync<UsersInChats>(_context);
            }
        }

        public IRepositoryAsync<ReadMessages> ReadMessages
        {
            get
            {
                return _readMessages ??= new BaseRepositoryAsync<ReadMessages>(_context);
            }
        }


        public async Task<bool> SaveAsync(Guid userId)
        {
            var entities = _context.ChangeTracker.Entries();
            if (!entities.Any()) 
                return Convert.ToBoolean(await _context.SaveChangesAsync());

            foreach (var entity in entities)
            {
                if (entity.Entity is not BaseEntity baseEntity) continue;

                if (entity.State == EntityState.Added)
                {
                    baseEntity.CreatedBy = userId;
                    baseEntity.CreatedAt = DateTime.Now;
                }
                else if (entity.State == EntityState.Modified)
                {
                    baseEntity.UpdatedBy = userId;
                    baseEntity.UpdatedAt = DateTime.Now;
                }
            }
            return Convert.ToBoolean(await _context.SaveChangesAsync());
        }
    }
}
