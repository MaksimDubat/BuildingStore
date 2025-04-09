using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands
{
    public class CreateCartCommand : IRequest
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        public CreateCartCommand(int userId)
        {
            UserId = userId;
        }
    }
}
