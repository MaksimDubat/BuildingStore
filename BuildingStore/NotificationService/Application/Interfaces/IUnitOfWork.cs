using NotificationService.Domain.Collections;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс паттерна UnitOfWork.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Базовый репозиторий по работе с CRUD операциями.
        /// </summary>
        IBaseRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Репозиторий по работе с сообщениями.
        /// </summary>
        IEmailMessageRepository EmailMessages { get; }
    }
}
