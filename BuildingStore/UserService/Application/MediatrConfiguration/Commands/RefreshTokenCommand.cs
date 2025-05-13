using MediatR;
using System.Security.Claims;
using UserService.Application.Common;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления токенов.
    /// </summary>
    public class RefreshTokenCommand : IRequest<Result<RefreshTokensResult>>
    {
        public string Refresh { get; set; }
        public string ExpiresAtString { get; set; }
        public ClaimsPrincipal User { get; set; }

        public RefreshTokenCommand(string refresh, string expiresAtString, ClaimsPrincipal user)
        {
            Refresh = refresh;
            ExpiresAtString = expiresAtString;
            User = user;
        }
    }

}
