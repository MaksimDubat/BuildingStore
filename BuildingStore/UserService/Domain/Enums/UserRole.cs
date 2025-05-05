namespace UserService.Domain.Enums
{
    /// <summary>
    /// Роли пользователей.
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Администратор.
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Пользователь.
        /// </summary>
        User = 2,

        /// <summary>
        /// Гость.
        /// </summary>
        Guest = 3,

        /// <summary>
        /// Менеджер.
        /// </summary>
        Manager = 4,
    }
}
