using BCrypt.Net;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    /// <summary>
    /// Сервис для хеширования паролей.
    /// </summary>
    public class PasswordHasherService : IPasswordHasherService
    {
        /// <inheritdoc/>
        public string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        /// <inheritdoc/>
        public bool VerifyPasswordHash(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
