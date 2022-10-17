using System.ComponentModel.DataAnnotations;

namespace IdentityServerJWT.API.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientURI { get; set; }
    }
}
