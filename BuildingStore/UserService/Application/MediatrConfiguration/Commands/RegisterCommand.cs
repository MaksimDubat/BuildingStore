using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Models;

namespace UserService.Application.MediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды регистрации пользователя.
    /// </summary>
    public class RegisterCommand : IRequest<Result<UserDto>>
    {
        /// <summary>
        /// Модель регистрации.
        /// </summary>
        public RegistrationModel Model { get; set; }

        public RegisterCommand(RegistrationModel model)
        {
            Model = model;
        }
    }
}
