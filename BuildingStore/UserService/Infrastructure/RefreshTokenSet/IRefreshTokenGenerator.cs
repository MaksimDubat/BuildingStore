namespace UserService.Infrastructure.RefreshTokenSet
{
    /// <summary>
    /// Интерфейс для создания и проверки Refresh токена.
    /// </summary>
    public interface IRefreshTokenGenerator
    {
        /// <summary>
        /// Генерация refresh токена.
        /// </summary>
        RefreshToken GenerateRefreshToken();

        /// <summary>
        /// Проверка refresh токена.
        /// </summary>
        /// <param name="refreshToken"></param>
        bool IsRefreshTokenValid(RefreshToken refreshToken);
    }
}
