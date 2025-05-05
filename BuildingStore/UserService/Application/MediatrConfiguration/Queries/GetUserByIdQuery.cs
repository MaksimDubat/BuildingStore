using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение пользователя по идентификатору.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserByIdQuery(int UserId) : IRequest<UserDto>;
}
