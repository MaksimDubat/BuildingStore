namespace UserService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс паттерна UnityOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Репозиторий по работе с пользователями.
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Базовый репозиторий.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        IBaseRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Заврешение операции.
        /// </summary>
        /// <param name="cancellation"></param>
        Task<int> CompleteAsync(CancellationToken cancellation);
    }
}
