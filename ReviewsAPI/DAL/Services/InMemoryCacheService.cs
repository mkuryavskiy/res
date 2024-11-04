using System;
using System.Data;
using System.Threading.Tasks;
using DAL.Models;
using Dapper;
using Microsoft.Extensions.Caching.Memory;

namespace DAL.Services
{
    public class InMemoryCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IDbConnection _dbConnection;
        private readonly MemoryCacheEntryOptions _defaultCacheOptions;

        public InMemoryCacheService(IMemoryCache cache, IDbConnection dbConnection)
        {
            _cache = cache;
            _dbConnection = dbConnection;
            _defaultCacheOptions = new MemoryCacheEntryOptions
            {
                Size = 1, // Обмеження за розміром
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }

        // Метод для отримання огляду з кешу або бази даних
        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await GetOrSetCacheAsync(id, async () =>
            {
                string sql = @"
            SELECT r.*, u.* 
            FROM Reviews r
            LEFT JOIN Users u ON r.UserId = u.Id
            WHERE r.Id = @Id";

                var review = await _dbConnection.QueryAsync<Review, User, Review>(
                    sql,
                    (r, u) =>
                    {
                        r.User = u;  // Призначаємо користувача відгуку
                        return r;
                    },
                    new { Id = id }
                );

                return review.FirstOrDefault();
            });
        }

        // Метод для отримання користувача з кешу або бази даних
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await GetOrSetCacheAsync(id, async () =>
            {
                string sql = @"SELECT * FROM Users WHERE Id = @Id";
                return await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            });
        }

        // Загальний метод для отримання даних з кешу або бази даних
        private async Task<T> GetOrSetCacheAsync<T>(int id, Func<Task<T>> fetchData)
        {
            if (!_cache.TryGetValue(id, out T cachedData))
            {
                // Отримання даних з бази даних, якщо даних немає в кеші
                cachedData = await fetchData();

                // Збереження в кеші, якщо дані були отримані
                if (cachedData != null)
                {
                    _cache.Set(id, cachedData, _defaultCacheOptions);
                }
            }
            return cachedData;
        }
    }
}
