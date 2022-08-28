using System.ComponentModel.DataAnnotations;

namespace IdentityServerJWT.API.Models
{
    public class UpdateUserPassword
    {
        [Required]
        public string UserId  { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
