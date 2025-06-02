using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Application.Models;
using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserProfileCacheService _userProfileCacheService;
     
        public AccountController(IMediator mediator, IUserProfileCacheService userProfileCacheService)
        {
            _mediator = mediator;
            _userProfileCacheService = userProfileCacheService;
        }

        /// <summary>
        /// Осуществление регистрации пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model, CancellationToken cancellation)
        {
            var command = await _mediator.Send(new RegisterCommand(model), cancellation);

            var result = command.Data;

            await _userProfileCacheService.SetProfileAsync(result.Id, result, TimeSpan.FromHours(3), cancellation);

            return Ok(new { command.Message });
        }

        /// <summary>
        /// Осуществление регистрации менеджеров.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("register-manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegistrationModel model, CancellationToken cancellation)
        {
            var command = await _mediator.Send(new RegisterManagersCommand(model), cancellation);

            var result = command.Data;

            await _userProfileCacheService.SetProfileAsync(result.Id, result, TimeSpan.FromHours(3), cancellation);

            return Ok(new { command.Message });
        }

        /// <summary>
        /// Осуществление входа пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellation)
        {
            var command = await _mediator.Send(new LoginCommand(model), cancellation);

            var loginResult = command.Data;

            await _userProfileCacheService.GetOrSetProfileAsync(loginResult.User.Id, TimeSpan.FromHours(3), cancellation);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };

            Response.Cookies.Append("cookies", loginResult.AccessToken, cookieOptions);
            Response.Cookies.Append("fresh-cookies", loginResult.RefreshToken.Refresh, cookieOptions);
            Response.Cookies.Append("fresh-time-cookies", loginResult.RefreshToken.Expires.ToString("O"), cookieOptions);

            return Ok(new { command.Message });
        }

        /// <summary>
        /// Осуществление выхода пользователя.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellation)
        {
            var command = await _mediator.Send(new LogoutCommand(User), cancellation);

            var userId = command.Data;

            await _userProfileCacheService.RemoveProfileAsync(userId, cancellation);

            Response.Cookies.Delete("cookies");
            Response.Cookies.Delete("fresh-cookies");
            Response.Cookies.Delete("fresh-time-cookies");

            return Ok(new { command.Message });
        }

        /// <summary>
        /// Обновление токенов.
        /// </summary>
        /// <param name="cancellationToken"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokens(CancellationToken cancellationToken)
        {
            var refresh = Request.Cookies["fresh-cookies"];
            var expiresAtString = Request.Cookies["fresh-time-cookies"];

            var command = await _mediator.Send(new RefreshTokenCommand(refresh, expiresAtString, User), cancellationToken);

            var refreshTokens = command.Data;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };

            Response.Cookies.Append("cookies", refreshTokens.AccessToken, cookieOptions);
            Response.Cookies.Append("fresh-cookies", refreshTokens.RefreshToken.Refresh, cookieOptions);
            Response.Cookies.Append("fresh-time-cookies", refreshTokens.RefreshToken.Expires.ToString("O"), cookieOptions);

            return Ok(new { command.Message });
        }
    }
}
