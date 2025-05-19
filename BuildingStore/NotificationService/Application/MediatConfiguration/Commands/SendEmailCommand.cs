using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.MediatConfiguration.Commands
{
    /// <summary>
    /// Модель команды отправки сообщения.
    /// </summary>
    public record SendEmailCommand(IEnumerable<UserResultDto> Users, string Subject) : IRequest<Result<IEnumerable<UserResultDto>>>;

}
