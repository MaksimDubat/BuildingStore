using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.Application.DTOs
{
    /// <summary>
    /// Результат обновления токенов.
    /// </summary>
    public class RefreshTokensResult
    {
        /// <summary>
        /// Сессионный токен.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh токен.
        /// </summary>
        public RefreshToken RefreshToken { get; set; }
    }
}
