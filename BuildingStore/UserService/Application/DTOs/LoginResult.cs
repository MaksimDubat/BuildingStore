using UserService.Domain.Entities;
using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.Application.DTOs
{
    /// <summary>
    /// Результат входа.
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// Сессионный токен.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh токен.
        /// </summary>
        public RefreshToken RefreshToken { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public AppUser User { get; set; }
    }
}
