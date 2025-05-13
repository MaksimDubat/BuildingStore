using MediatR;
using System.Security.Claims;
using UserService.Application.Common;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель комнады для выхода пользователя. 
    /// </summary>
    public record LogoutCommand(ClaimsPrincipal User) : IRequest<Result<int>>;
}
