using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Entities.TripEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.DAL.Data
{
    public class ApplicationDbContext: DbContext //IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<TripUser> TripUsers { get; set; }
        public DbSet<AvailableSeats> AvailableSeats { get; set; }
        public DbSet<UserDocuments> UserDocuments  { get; set; }
        public DbSet<CarDocuments> CarDocuments { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<ReadNotifications> ReadNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber).IsUnique();
           // builder.Entity<ApplicationUser>().Property(x=>x.DrivingLicense).IsRequired(false);
           builder.Entity<Car>().HasIndex(x => x.RegistNum);
        }
    }
}
