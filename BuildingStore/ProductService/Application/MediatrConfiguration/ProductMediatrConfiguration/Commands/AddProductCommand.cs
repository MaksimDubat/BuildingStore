using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Models.RequestModels;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для добавления продукта.
    /// </summary>
    public class AddProductCommand : IRequest
    {
        /// <summary>
        /// Продукт.
        /// </summary>
        public ProductDto Product { get; set; }

        public AddProductCommand() { }

        public AddProductCommand(ProductDto product)
        {
            Product = product;
        }
    }
}
