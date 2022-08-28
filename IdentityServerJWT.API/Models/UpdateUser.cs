using System.ComponentModel.DataAnnotations;

namespace IdentityServerJWT.API.Models
{
    public class UpdateUser
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? FirstName { get; set; }
    }
}
