using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса аунтефикации.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Осуществляет вход пользователя.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="cancellation">Токен отмены операции.</param>
        Task<string> SignInAsync(string email, string password, CancellationToken cancellation);

        /// <summary>
        /// Осуществляет регистрацию пользователя.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="cancellation">Токен отмены операции.</param>
        Task<User> RegisterAsync(string name, string email, string password, CancellationToken cancellation);
    }
}
