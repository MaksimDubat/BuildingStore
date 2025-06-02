using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces
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

        /// <summary>
        /// Получение товаров без скидок.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cancellation"></param>
        Task<IEnumerable<Product>> GetProductsWithoutSale(CancellationToken cancellation);

        /// <summary>
        /// Получение товаров со скидкой. 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsWithSale(CancellationToken cancellation);

        /// <summary>
        /// Получение продуктов с истекшими скидками.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetExpiredSalesAsync(CancellationToken cancellation);

    }
}
