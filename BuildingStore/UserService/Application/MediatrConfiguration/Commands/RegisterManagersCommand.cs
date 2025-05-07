using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Models;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды регистрации менеджеров.
    /// </summary>
    public class RegisterManagersCommand: IRequest<Result<UserDto>>
    {
        /// <summary>
        /// Модель регистрации.
        /// </summary>
        public RegistrationModel Model { get; set; }

        public RegisterManagersCommand(RegistrationModel model)
        {
            Model = model;
        }
    }
}
