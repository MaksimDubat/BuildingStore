using UserService.Domain.Enums;

namespace UserService.Domain.Entities
{
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public UserRole Role { get; set; }
    }
}
