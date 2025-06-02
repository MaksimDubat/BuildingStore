namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO сущности пользователя.
    /// </summary>
    public class UserResultDto
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string UserEmail { get; set; }
    }
}
