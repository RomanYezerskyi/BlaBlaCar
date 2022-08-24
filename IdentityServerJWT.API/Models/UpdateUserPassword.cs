namespace IdentityServerJWT.API.Models
{
    public class UpdateUserPassword
    {
        public string UserId  { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
