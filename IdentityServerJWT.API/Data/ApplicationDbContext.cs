using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerJWT.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber).IsUnique();
            base.OnModelCreating(builder);
        }
    }
}
