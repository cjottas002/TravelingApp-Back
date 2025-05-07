using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TravelingApp.Application.Common.Interfaces;

namespace TravelingApp.Infraestructure.Services
{
    public class RedisCacheService(IDistributedCache _cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var data = await _cache.GetStringAsync(key);
                if (string.IsNullOrEmpty(data))
                    return default;

                return JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public Task SetAsync<T>(string key, T value, double? slidingExpirationMinutes = null, double? absoluteExpirationRelativeToNowMinutes = null)
        {
            var slidingTs = slidingExpirationMinutes.HasValue
                ? TimeSpan.FromMinutes(slidingExpirationMinutes.Value)
                : (TimeSpan?)null;

            var absoluteTs = absoluteExpirationRelativeToNowMinutes.HasValue
                ? TimeSpan.FromMinutes(absoluteExpirationRelativeToNowMinutes.Value)
                : (TimeSpan?)null;

            return SetAsync(key, value, slidingTs, absoluteTs);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();

                if (slidingExpiration.HasValue)
                    options.SetSlidingExpiration(slidingExpiration.Value);

                if (absoluteExpirationRelativeToNow.HasValue)
                    options.SetAbsoluteExpiration(absoluteExpirationRelativeToNow.Value);

                var data = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(key, data, options);
            }
            catch (Exception ex)
            {
            }
        }



        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
