namespace ProductService.Domain.Interfaces
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
        /// Завершение операции.
        /// </summary>
        /// <param name="cancellation"></param>
        Task<int> CompleteAsync(CancellationToken cancellation);
    }
}
