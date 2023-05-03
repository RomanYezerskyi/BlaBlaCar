namespace BlaBlaCar.API
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public string WebClientUrl { get; set; }
        public string MobileClientUrl { get; set; }
    }
}
