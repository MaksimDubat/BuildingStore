
namespace NotificationService.Application.DTOs
{
    /// <summary>
    /// DTO email сообщений.
    /// </summary>
    public class EmailMessageDto
    {
        /// <summary>
        /// Идентификатор сообщения.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public string Body { get; set; }
    }
}
