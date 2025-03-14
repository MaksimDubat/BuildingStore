using MediatR;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для удаления продукта.
    /// </summary>
    public class DeleteProductCommand : IRequest<string>
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int Id { get; set; } 
        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
