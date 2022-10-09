using System.ComponentModel.DataAnnotations;

namespace IdentityServerJWT.API.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ClientURI { get; set; }
    }
}
