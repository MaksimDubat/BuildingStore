namespace UserService.Application.Models
{
    /// <summary>
    /// Модель регистрации пользователя.
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Подтверждение пароля пользователя.
        /// </summary>
        public string ConfirmPassword { get; set; }

    }
}
