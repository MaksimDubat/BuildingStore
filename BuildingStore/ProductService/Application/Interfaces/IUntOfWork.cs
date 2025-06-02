namespace ProductService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс паттерна UnityOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Репозиторий по работе с продуктом.
        /// </summary>
        IProductRepository Products { get; }

        /// <summary>
        /// Репозиторий по работе с категориями.
        /// </summary>
        ICategoryRepository Categories { get; }

        /// <summary>
        /// Репозиторий по работе с продуктами в корзине.
        /// </summary>
        ICartItemRepository CartItems { get; }

        /// <summary>
        /// Репозиторий по работе с корзиной.
        /// </summary>
        ICartRepository Carts { get; }

        /// <summary>
        /// Репозиторий по работе с заказми.
        /// </summary>
        IOrderRepository Orders { get; }

        /// <summary>
        /// Завершение операции.
        /// </summary>
        /// <param name="cancellation"></param>
        Task<int> CompleteAsync(CancellationToken cancellation);
    }
}
