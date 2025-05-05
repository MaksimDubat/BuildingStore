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
    [Route("api/auth")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Осуществление регистрации пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model, CancellationToken cancellation)
        {
            await _mediator.Send(new RegisterCommand(model), cancellation);
            return Ok("Registration done");
        }

        /// <summary>
        /// Осуществление регистрации менеджеров.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("registermanager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegistrationModel model, CancellationToken cancellation)
        {
            await _mediator.Send(new RegisterManagersCommand(model), cancellation);
            return Ok("Registration done");
        }

        /// <summary>
        /// Осуществление входа пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellation)
        {
            await _mediator.Send(new LoginCommand(model), cancellation);
            return Ok("Done");
        }

        /// <summary>
        /// Осуществление выхода пользователя.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellation)
        {
            await _mediator.Send(new LogoutCommand(), cancellation);
            return Ok("bye");
        }

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(), cancellation);
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
        [HttpGet("user")]
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
        [HttpDelete("deleteuser")]
        public async Task<ActionResult> DeleteUser(int userId, CancellationToken cancellation)
        {
            await _mediator.Send(new DeleteUserCommand(userId), cancellation);
            return Ok("Deleted");
        }

        /// <summary>
        /// Обновление токенов.
        /// </summary>
        /// <param name="cancellationToken"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokens(CancellationToken cancellationToken)
        {
            await _mediator.Send(new RefreshTokenCommand(), cancellationToken);
            return Ok("Session refreshed");
        }
    }
}
