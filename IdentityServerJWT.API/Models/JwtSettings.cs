namespace IdentityServerJWT.API.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public string Secret { get; set; }

        public int ExpirationInMinutes { get; set; }
        public int RefreshInMinutes { get; set; }
    }
}
