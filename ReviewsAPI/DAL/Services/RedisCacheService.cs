using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _defaultCacheOptions;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
            _defaultCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
        }

        public async Task SetCacheAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null)
        {
            var serializedData = JsonSerializer.Serialize(value);
            options ??= _defaultCacheOptions; // Використовуємо стандартні параметри, якщо не задано
            await _cache.SetStringAsync(key, serializedData, options);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            return data == null ? default : JsonSerializer.Deserialize<T>(data);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
