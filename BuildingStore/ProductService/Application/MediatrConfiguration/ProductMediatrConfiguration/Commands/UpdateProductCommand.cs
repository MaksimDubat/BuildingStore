using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления продукта.
    /// </summary>
    public class UpdateProductCommand : IRequest<string>
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Продукт.
        /// </summary>
        public ProductDto Product { get; set; } 
        public UpdateProductCommand(int id, ProductDto product)
        {
            Id = id;
            Product = product;
        }
    }
}
