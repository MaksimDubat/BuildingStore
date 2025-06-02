using MongoDB.Bson;
using NotificationService.Domain.Collections;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс по работе с email сообщениями.
    /// </summary>
    public interface IEmailMessageRepository : IBaseRepository<EmailMessage>
    {
        /// <summary>
        /// Получение сообщения по его теме.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="cancellation"></param>
        Task<EmailMessage> GetMessageBySubjectAsync(string subject, CancellationToken cancellation);

        /// <summary>
        /// Получение послденего добавленного сообщения.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<EmailMessage> GetLatestMessageAsync(CancellationToken cancellation);
    }
}
