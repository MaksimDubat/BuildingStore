using MediatR;
using UserService.Application.Models;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для осуществления входа пользователя.
    /// </summary>
    public class LoginCommand : IRequest
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
