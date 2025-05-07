namespace TravelingApp.CrossCutting.Configuration
{
    public class RedisOptions
    {
        public double? SlidingExpiration { get; set; }
        public double? AbsoluteExpirationRelativeToNow { get; set; }
    }
}
