using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Application.Models;

namespace UserService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers([FromQuery] int page, [FromQuery] int size, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(page, size), cancellation);
            return Ok(result);
        }

        /// <summary>
        /// Получение менеджеров.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetManagers(CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetManagersQuery(), cancellation);
            return Ok(result);
        }

        /// <summary>
        /// Получение пользователя по идентифкатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(int userId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId), cancellation);
            return Ok(result);
        }

        /// <summary>
        /// Удаление пользователя по идентифкатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteUserCommand(userId), cancellation);
            return Ok(new { result.Message });
        }
    }
}
