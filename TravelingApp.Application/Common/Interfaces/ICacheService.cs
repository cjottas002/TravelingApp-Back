using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingApp.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, double? slidingExpirationMinutes = null, double? absoluteExpirationRelativeToNowMinutes = null);
        Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpirationRelativeToNow = null);
        Task RemoveAsync(string key);
    }
}
