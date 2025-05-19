using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace NotificationService.Infrastructure.RedisCache
{
    /// <summary>
    /// Сервис по работе с кешем.
    /// </summary>
    public class UserEmailCacheService : IUserEmailCacheService
    {
        private readonly IDistributedCache _cache;
        private const string prefix = "user_emails_";
        private const string IndexKey = "user_email_ids";

        private readonly IUnitOfWork _unitOfWork;

        public UserEmailCacheService(IDistributedCache cache, IUnitOfWork unitOfWork)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        private readonly ConcurrentDictionary<int, string> _emails = new();

        /// <inheritdoc/>
        public async Task SetProfileAsync(int userId, string userEmail, TimeSpan ttl, CancellationToken cancellation)
        {
            var key = $"{prefix}{userId}";
            var json = JsonSerializer.Serialize(userEmail);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl,
            };

            await _cache.SetStringAsync(key, json, options, cancellation);

            var indexJson = await _cache.GetStringAsync(IndexKey, cancellation);

            HashSet<int> index;

            if (string.IsNullOrEmpty(indexJson))
            {
                index = new HashSet<int>();
            }
            else
            {
                index = JsonSerializer.Deserialize<HashSet<int>>(indexJson)!;
            }

            index.Add(userId);

            var updatedIndex = JsonSerializer.Serialize(index);

            await _cache.SetStringAsync(IndexKey, updatedIndex, cancellation);
        }

        /// <inheritdoc/>
        public async Task RemoveEmailsAsync(CancellationToken cancellation)
        {
            var indexJson = await _cache.GetStringAsync(IndexKey, cancellation);

            if (!string.IsNullOrEmpty(indexJson))
            {
                var index = JsonSerializer.Deserialize<HashSet<int>>(indexJson)!;

                foreach (var userId in index)
                {
                    var key = $"{prefix}{userId}";

                    await _cache.RemoveAsync(key, cancellation);
                }

                await _cache.RemoveAsync(IndexKey, cancellation);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserResultDto>> GetAllEmailsAsync(CancellationToken cancellation)
        {
            var indexJson = await _cache.GetStringAsync(IndexKey, cancellation);

            var index = JsonSerializer.Deserialize<HashSet<int>>(indexJson);

            var result = new List<UserResultDto>();

            foreach (var id in index)
            {
                var key = $"{prefix}{id}";

                var emailJson = await _cache.GetStringAsync(key, cancellation);

                if (!string.IsNullOrEmpty(emailJson))
                {
                    var email = JsonSerializer.Deserialize<string>(emailJson);

                    if (!string.IsNullOrEmpty(email))
                    {
                        result.Add(new UserResultDto
                        {
                            Id = id,
                            UserEmail = email
                        });
                    }
                }
            }

            return result;
        }
    }
}
