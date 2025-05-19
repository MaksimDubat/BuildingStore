using NotificationService.Application.DTOs;
using UserService.Application.DTOs;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс по работе с кешированием email-ов.
    /// </summary>
    public interface IUserEmailCacheService
    {
        /// <summary>
        /// Получение всех email-ов пользователей.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IEnumerable<UserResultDto>> GetAllEmailsAsync(CancellationToken cancellation);

        /// <summary>
        /// Сохранение профиля в кеш.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profile"></param>
        /// <param name="ttl"></param>
        /// <param name="cancellation"></param>
        Task SetProfileAsync(int userId, string userEmail, TimeSpan ttl, CancellationToken cancellation);

        /// <summary>
        /// Удаление профиля из кеша.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task RemoveEmailsAsync(CancellationToken cancellation);
    }
}
