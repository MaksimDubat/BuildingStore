using NotificationService.Domain.Collections;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория по работе с email.
    /// </summary>
    public interface IEmailsToSentRepository : IBaseRepository<EmailsToSent>
    {
        /// <summary>
        /// Получение по email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellation"></param>
        Task<EmailsToSent> GetByEmailAsync(string email, CancellationToken cancellation);
    }
}
