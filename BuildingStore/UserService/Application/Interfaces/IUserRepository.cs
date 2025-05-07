using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория по работе с пользователями.
    /// </summary>
    public interface IUserRepository : IBaseRepository<AppUser>
    {
        /// <summary>
        /// Получение пользователя по email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellation);

        /// <summary>
        /// Проверка на наличие уже существующего пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellation"></param>
        Task<bool> IsUserExistOrDuplicateAsync(AppUser user, CancellationToken cancellation);

        /// <summary>
        /// Получение пользователей менеджеров.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellation"></param>
        Task<IEnumerable<AppUser>> GetManagersAsync(CancellationToken cancellation);

    }
}
