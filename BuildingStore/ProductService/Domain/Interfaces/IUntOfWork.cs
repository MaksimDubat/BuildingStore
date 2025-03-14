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
        /// Завершение операции.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<int> CompleteAsync(CancellationToken cancellation);
    }
}
