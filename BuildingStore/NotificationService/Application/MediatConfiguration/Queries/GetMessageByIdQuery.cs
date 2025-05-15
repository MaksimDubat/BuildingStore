using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.MediatConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение сообщения по идентифкатору.
    /// </summary>
    /// <param name="MessageId"></param>
    public record GetMessageByIdQuery(string MessageId) : IRequest<Result<EmailMessageDto>>;

}
