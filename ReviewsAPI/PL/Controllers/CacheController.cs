using DAL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly InMemoryCacheService _inMemoryCacheService;
        private readonly RedisCacheService _redisCacheService;

        public CacheController(InMemoryCacheService inMemoryCacheService, RedisCacheService redisCacheService)
        {
            _inMemoryCacheService = inMemoryCacheService;
            _redisCacheService = redisCacheService;
        }

        [HttpGet("review/{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _inMemoryCacheService.GetReviewByIdAsync(id);
            return review == null ? NotFound() : Ok(review);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _inMemoryCacheService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("redis-cache/review/{id}")]
        public async Task<IActionResult> SetReviewInRedis(int id)
        {
            var review = await _inMemoryCacheService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();

            await _redisCacheService.SetCacheAsync($"review_{id}", review, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
            return Ok();
        }

        [HttpGet("redis-cache/review/{id}")]
        public async Task<IActionResult> GetReviewFromRedis(int id)
        {
            var review = await _redisCacheService.GetCacheAsync<Review>($"review_{id}");
            return review == null ? NotFound() : Ok(review);
        }
    }
}
