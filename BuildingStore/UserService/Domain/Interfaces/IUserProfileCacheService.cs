using UserService.Application.DTOs;
using UserService.Application.Mappings;
using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс по работе с кешированием профиля.
    /// </summary>
    public interface IUserProfileCacheService
    {
        /// <summary>
        /// Получение профиля из кеша по идентифкатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        Task<UserDto> GetProfileAsync(int userId, CancellationToken cancellation);

        /// <summary>
        /// Сохранение профиля в кеш.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profile"></param>
        /// <param name="ttl"></param>
        /// <param name="cancellation"></param>
        Task SetProfileAsync(int userId, UserDto profile, TimeSpan ttl, CancellationToken cancellation);

        /// <summary>
        /// Удаление профиля из кеша.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task RemoveProfileAsync(int userId, CancellationToken cancellation);
    }
}
