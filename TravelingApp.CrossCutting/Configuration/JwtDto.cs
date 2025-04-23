namespace TravelingApp.CrossCutting.Configuration
{
    public class JwtDto
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }
}
