using MongoDB.Bson;

namespace NotificationService.Domain.Collections
{
    /// <summary>
    /// Коллекция пользователей.
    /// </summary>
    public class EmailsToSent
    {
        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string Email { get; set; }
    }
}
