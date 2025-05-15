using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.DataBase;

namespace NotificationService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с CRUD-операциями.
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public BaseRepository(NotificationDbContext context)
        {
            _collection = context.GetCollection<T>();
        }

        /// <inheritdoc/>
        public async Task AddEntityAsync(T entity, CancellationToken cancellation)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellation);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(ObjectId id, CancellationToken cancellation)
        {
            var filter = Builders<T>.Filter.Eq("_id",id);
            await _collection.DeleteOneAsync(filter, cancellationToken: cancellation);
        }

        /// <inheritdoc/>
        public async Task<List<T>> GetAllAsync(Func<IFindFluent<T, T>, IFindFluent<T, T>> queryBuilder, CancellationToken cancellation)
        {
            var filter = Builders<T>.Filter.Empty;

            var find = _collection.Find(filter);

            var query = queryBuilder(find);

            return await query.ToListAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task<T> GetByIdAsync(ObjectId id, CancellationToken cancellation)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellation);

        }

        /// <inheritdoc/>
        public async Task UpdateAsync(T entity, CancellationToken cancellation)
        {
            var idProp = typeof(T).GetProperty("Id");
            var idValue = idProp.GetValue(entity);

            var filter = Builders<T>.Filter.Eq("_id", idValue);
            await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellation);
        }
    }
}
