using UserService.Domain.Enums;

namespace UserService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности пользователя.
    /// </summary>
    public class UserDto
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
        /// Роль.
        /// </summary>
        public string Role { get; set; }

    }
}
