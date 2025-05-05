using MediatR;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель комнады для выхода пользователя. 
    /// </summary>
    public record LogoutCommand() : IRequest;
}
