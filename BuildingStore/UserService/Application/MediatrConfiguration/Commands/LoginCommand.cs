using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Models;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для осуществления входа пользователя.
    /// </summary>
    public class LoginCommand : IRequest<Result<LoginResult>>
    {
        /// <summary>
        /// Модель входа.
        /// </summary>
        public LoginModel Model { get; set; }

        public LoginCommand(LoginModel model)
        {
            Model = model;
        }
    }
}
