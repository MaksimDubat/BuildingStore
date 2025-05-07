using MediatR;
using System.Security.Claims;
using UserService.Application.Common;
using UserService.Application.MediatrConfiguration.Commands;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса выхода пользователя.
    /// </summary>
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Result<int>.Failure("Invalid user ID.");
            }

            return Result<int>.Success(userId, "Bye");
        }
    }
}
