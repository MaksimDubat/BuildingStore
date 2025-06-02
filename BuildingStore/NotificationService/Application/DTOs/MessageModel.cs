namespace NotificationService.Application.DTOs
{
    /// <summary>
    /// DTO для обновления сообщения.
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообшения.
        /// </summary>
        public string Body { get; set; }
    }
}
