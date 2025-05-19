using MongoDB.Bson;
using MongoDB.Driver;
using NotificationService.Domain.Collections;
using System.Linq.Expressions;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс общего репозитория по работе с CRUD-операциями.
    /// </summary>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Добавление сущности.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellation"></param>
        Task AddEntityAsync(T entity, CancellationToken cancellation);

        /// <summary>
        /// Получение всех сущностей.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(Func<IFindFluent<T, T>, IFindFluent<T, T>> queryBuilder, CancellationToken cancellation);

        /// <summary>
        /// Обновление сущности.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellation"></param>
        Task UpdateAsync(T entity, CancellationToken cancellation);

        /// <summary>
        /// Удаление сущности.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        Task DeleteAsync(ObjectId id, CancellationToken cancellation);

        /// <summary>
        /// Получение сущности по идентифкатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        Task<T> GetByIdAsync(ObjectId id, CancellationToken cancellation);

        /// <summary>
        /// Проверка записей.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellation"></param>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation);
    }
}
