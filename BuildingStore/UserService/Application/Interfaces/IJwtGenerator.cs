using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
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
        string GenerateToken(AppUser user);
    }
}
