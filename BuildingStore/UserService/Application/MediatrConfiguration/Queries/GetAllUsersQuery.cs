using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех пользователей.
    /// </summary>
    public record GetAllUsersQuery() : IRequest<IEnumerable<UserDto>>;
}
