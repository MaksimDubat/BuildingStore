
using MongoDB.Driver;
using NotificationService.Domain.Collections;


namespace NotificationService.Domain.DataBase
{
    /// <summary>
    /// Контекст для работы с БД.
    /// </summary>
    public class NotificationDbContext
    {
        private readonly IMongoDatabase _database;

        public NotificationDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName = null)
        {
            return _database.GetCollection<T>(collectionName ?? typeof(T).Name);
        }
    }
}
