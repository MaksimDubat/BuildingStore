using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение менеджеров.
    /// </summary>
    public record GetManagersQuery() : IRequest<IEnumerable<UserDto>>;
}
