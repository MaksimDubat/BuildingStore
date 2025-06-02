using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.MediatConfiguration.Commands
{
    /// <summary>
    /// Модель команды отправки сообщения.
    /// </summary>
    public record SendEmailCommand(string Subject) : IRequest<Result>;

}
