using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;
using NotificationService.Domain.Collections;

namespace NotificationService.Application.MediatConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех сообщений.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public record GetAllMessagesQuery(int PageNumber, int PageSize) : IRequest<Result<IEnumerable<EmailMessageDto>>>;

}