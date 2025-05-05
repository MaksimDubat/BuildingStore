using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория по работе с пользователями.
    /// </summary>
    public interface IUserRepository : IBaseRepository<User>
    {
        /// <summary>
        /// Получение пользователя по email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<User> GetByEmailAsync(string email, CancellationToken cancellation);

        /// <summary>
        /// Проверка на наличие уже существующего пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellation"></param>
        Task<bool> IsUserExistOrDuplicateAsync(User user, CancellationToken cancellation);

        /// <summary>
        /// Получение пользователей менеджеров.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellation"></param>
        Task<IEnumerable<User>> GetManagersAsync(CancellationToken cancellation);
    }
}
