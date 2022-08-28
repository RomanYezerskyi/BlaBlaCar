using System.ComponentModel.DataAnnotations;

namespace IdentityServerJWT.API.Models
{
    public class UserChangeRoleModel
    {
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
