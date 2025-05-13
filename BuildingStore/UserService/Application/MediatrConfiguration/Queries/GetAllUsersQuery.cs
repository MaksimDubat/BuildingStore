using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех пользователей.
    /// </summary>
    public record GetAllUsersQuery(int PageNumber, int PageSize) : IRequest<Result<IEnumerable<UserDto>>>;
}
