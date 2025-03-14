using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для добавления продукта.
    /// </summary>
    public class AddProductCommand : IRequest<string>
    {
        /// <summary>
        /// Продукт.
        /// </summary>
        public ProductDto Product { get; set; }
        public AddProductCommand(ProductDto product)
        {
            Product = product;
        }
    }
}
