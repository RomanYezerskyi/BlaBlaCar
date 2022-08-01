using BlaBlaCar.DAL.Entities;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasIndex(x => x.Email);
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber);
            builder.Entity<Car>().HasIndex(x => x.RegistNum);
        }
    }
}
