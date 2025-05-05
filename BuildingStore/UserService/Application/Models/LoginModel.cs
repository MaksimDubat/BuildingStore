namespace UserService.Application.Models
{
    /// <summary>
    /// Модель для входа пользователя.
    /// </summary>
    public class LoginModel
    {
        /// <summary>   
        /// Е-mail.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}
