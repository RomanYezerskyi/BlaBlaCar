using Microsoft.AspNetCore.Identity;

namespace IdentityServerJWT.API.Models
{
    public class UserWithRolesModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public UserWithRolesModel(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            FirstName = user.FirstName;
        }
        
    }
}
