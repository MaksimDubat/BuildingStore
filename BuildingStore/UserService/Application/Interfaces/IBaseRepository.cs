using System.Linq.Expressions;

namespace UserService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс общего репозитория по работе с CRUD-операциями.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Получение всех сущностей с пагинацией
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="cancellation"></param>
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellation);

        /// <summary>
        /// Получение сущности по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        Task<T> GetAsync(int id, CancellationToken cancellation);

        /// <summary>
        /// Добавление сущности.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellation"></param>
        Task AddEntityAsync(T entity, CancellationToken cancellation);

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
        Task<T> DeleteAsync(int id, CancellationToken cancellation);

        /// <summary>
        /// Проверка наличия уже существующих записей.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellation"></param>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation);

    }
}
