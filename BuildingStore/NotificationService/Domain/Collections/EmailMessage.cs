using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.Domain.Collections
{
    /// <summary>
    /// Коллекция email сообщений.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Идентификатор сообщения.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public string Body { get; set; }


        /// <summary>
        /// Время создания.
        /// </summary>
        public DateTime CreatedAt = DateTime.Now;

    }
}
