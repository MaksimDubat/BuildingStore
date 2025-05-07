using MediatR;
using System.Security.Claims;
using UserService.Application.Common;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления токенов.
    /// </summary>
    public record RefreshTokenCommand(string Refresh, string ExpiresAtString, ClaimsPrincipal User) : IRequest<Result<RefreshTokensResult>>;

}
