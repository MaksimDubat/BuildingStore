using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.MediatConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления сообщения.
    /// </summary>
    public class UpdateMessageCommand : IRequest<Result>
    {
        public string MessageId { get; set; }

        public MessageModel Message { get; set; }

        public UpdateMessageCommand(string messageId, MessageModel message)
        {
            MessageId = messageId;
            Message = message;
        }
    }
}
