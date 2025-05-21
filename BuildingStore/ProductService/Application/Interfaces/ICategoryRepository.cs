using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces
{
    /// <summary>
    /// Репозиторий по работе с категориями.
    /// </summary>
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        /// <summary>
        /// Проверка входящих данных категории на наличие уже существующей.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="cancellation"></param>
        Task<bool> IsCategoryExistOrDuplicateAsync(Category category, CancellationToken cancellation);
    }
}
