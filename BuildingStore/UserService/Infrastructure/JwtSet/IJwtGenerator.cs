using UserService.Domain.Entities;

namespace UserService.Infrastructure.JwtSet
{
    /// <summary>
    /// Интерфейс сервиса генерации токенов.
    /// </summary>
    public interface IJwtGenerator
    {
        /// <summary>
        /// Метод создания токена.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        string GenerateToken(User user);
    }
}
