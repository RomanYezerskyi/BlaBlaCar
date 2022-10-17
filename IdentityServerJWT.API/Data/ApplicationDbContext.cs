using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Security.Claims;
using IdentityModel;

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

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            { 
                    Id = "e23e7753-c716-40d0-b91a-ca49b10e304a", 
                    Name = Constants.AdminRole, 
                    NormalizedName = Constants.AdminRole.ToUpper()
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "fe0d6a7a-c600-4026-a77c-f26a9c307390",
                Name = Constants.UserRole,
                NormalizedName = Constants.UserRole.ToUpper()
            });

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "admin@gmail.com".ToUpper(),
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "admin@gmail.com".ToUpper(),
                    FirstName = "admin",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "1111"),
                    PhoneNumber = "+380999999999"
                }
            );
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "e23e7753-c716-40d0-b91a-ca49b10e304a",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
            builder.Entity<IdentityUserClaim<string>>().HasData(
                new List<IdentityUserClaim<string>>()
                {
                   new IdentityUserClaim<string>()
                   {
                       Id = 1,
                       ClaimType = JwtClaimTypes.Name,
                       ClaimValue = "admin@gmail.com",
                       UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                   },
                   new IdentityUserClaim<string>()
                   {
                       Id = 2,
                       ClaimType = JwtClaimTypes.Id,
                       ClaimValue = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                       UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                   },
                   new IdentityUserClaim<string>()
                   {
                       Id = 3,
                       ClaimType = ClaimTypes.Email,
                       ClaimValue = "admin@gmail.com",
                       UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                   },
                   new IdentityUserClaim<string>()
                   {

                       Id = 4,
                       ClaimType = ClaimTypes.GivenName,
                       ClaimValue = "admin",
                       UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                   },
                   new IdentityUserClaim<string>()
                   {

                       Id = 5,
                       ClaimType = JwtClaimTypes.PhoneNumber,
                       ClaimValue = "+380999999999",
                       UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                   },
                   new IdentityUserClaim<string>()
                   {
                       Id = 6,
                       ClaimType = JwtClaimTypes.Role,
                       ClaimValue = Constants.AdminRole,
                       UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                   },
                }
                );


        }
    }
}
