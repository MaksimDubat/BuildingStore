using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.RedisCache
{
    /// <summary>
    /// Сервис по работе кешерования профиля.
    /// </summary>
    public class UserProfileCacheService : IUserProfileCacheService
    {
        private readonly IDistributedCache _cache;
        private const string prefix = "user_profile_";

        public UserProfileCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetProfileAsync(int userId, CancellationToken cancellation)
        {
            var key = $"Userservice_{prefix}{userId}";
            var json = await _cache.GetStringAsync(key, cancellation);

            try
            {
                return JsonSerializer.Deserialize<UserDto>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task RemoveProfileAsync(int userId, CancellationToken cancellation)
        {
            var key = $"{prefix}{userId}";
            await _cache.RemoveAsync(key, cancellation);
        }

        /// <inheritdoc/>
        public async Task SetProfileAsync(int userId, UserDto profile, TimeSpan ttl, CancellationToken cancellation)
        {
            var key = $"{prefix}{userId}";
            var json = JsonSerializer.Serialize(profile);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl,
            };

            await _cache.SetStringAsync(key, json, options, cancellation);
        }
    }
}
