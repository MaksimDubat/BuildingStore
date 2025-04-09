using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    /// <summary>
    /// Репозиторий по работе с продуктом.
    /// </summary>
    public interface IProductRepository : IBaseRepository<Product>
    {
        /// <summary>
        /// Получение товара по наименованию.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellation"></param>
        Task<Product> GetByNameAsync(string name, CancellationToken cancellation);

        /// <summary>
        /// Проверка входящих данных продукта на наличие уже существующего.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cancellation"></param>
        Task<bool> IsProductExistOrDuplicateAsync(Product product, CancellationToken cancellation);

    }
}
