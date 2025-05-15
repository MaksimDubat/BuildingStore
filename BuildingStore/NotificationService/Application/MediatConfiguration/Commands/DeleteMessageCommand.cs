using MediatR;
using NotificationService.Application.Common;

namespace NotificationService.Application.MediatConfiguration.Commands
{
    /// <summary>
    /// Модель запроса на удаление сообщения.
    /// </summary>
    public record DeleteMessageCommand(string MessageId) : IRequest<Result>;
}
