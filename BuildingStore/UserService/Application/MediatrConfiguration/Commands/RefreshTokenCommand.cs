using MediatR;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления токенов.
    /// </summary>
    public record RefreshTokenCommand() : IRequest;

}
