namespace UserService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс по работе с хешированными паролями.
    /// </summary>
    public interface IPasswordHasherService
    {
        /// <summary>
        /// Генерация хеша для пароля.
        /// </summary>
        /// <param name="password"></param>
        string GeneratePasswordHash(string password);

        /// <summary>
        /// Проверка пароля.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        bool VerifyPasswordHash(string password, string hashedPassword);
    }
}
