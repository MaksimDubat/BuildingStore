using System.Security.Cryptography;

namespace UserService.Infrastructure.RefreshTokenSet
{
    /// <summary>
    /// Сервис для создания и проверки Refresh токена.
    /// </summary>
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        /// <inheritdoc/>
        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Refresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7)
            };

            return refreshToken;
        }

        /// <inheritdoc/>
        public bool IsRefreshTokenValid(RefreshToken refreshToken)
        {
            return refreshToken != null && refreshToken.Expires > DateTime.UtcNow;
        }
    }
}
