using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;

namespace UserService.Application.MediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение менеджеров.
    /// </summary>
    public record GetManagersQuery(int PazeNumber, int PageSize) : IRequest<Result<IEnumerable<UserDto>>>;
}
