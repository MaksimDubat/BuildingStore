using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.RedisCache
{
    /// <summary>
    /// Сервис по работе кешерования профиля.
    /// </summary>
    public class UserProfileCacheService : IUserProfileCacheService
    {
        private readonly IDistributedCache _cache;
        private const string prefix = "user_profile_";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserProfileCacheService(IDistributedCache cache, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetProfileAsync(int userId, CancellationToken cancellation)
        {
            var key = $"{prefix}{userId}";
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

        /// <inheritdoc/>
        public async Task<UserDto> GetOrSetProfileAsync(int userId, TimeSpan ttl, CancellationToken cancellation)
        {
            var cached = await GetProfileAsync(userId, cancellation);

            if (cached != null)
            {
                return cached;
            }

            var user = await _unitOfWork.Users.GetAsync(userId, cancellation);

            if(user == null)
            {
                throw new KeyNotFoundException("Not found");
            }

            var profile = _mapper.Map<UserDto>(user);
            await SetProfileAsync(userId, profile, ttl, cancellation);

            return profile;
        }
    }
}
