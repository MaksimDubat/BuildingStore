using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.MediatConfiguration.Commands
{
    /// <summary>
    /// Модель команды для добавления сообщений.
    /// </summary>
    public record AddMessageCommand(MessageModel Model) : IRequest<Result<EmailMessageDto>>;

}
