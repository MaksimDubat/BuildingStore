using MediatR;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для удаления пользователя по идентифкатору.
    /// </summary>
    public record DeleteUserCommand(int UserId) : IRequest;
}
