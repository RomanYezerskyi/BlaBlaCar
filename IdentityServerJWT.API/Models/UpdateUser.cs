namespace IdentityServerJWT.API.Models
{
    public class UpdateUser
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
    }
}
