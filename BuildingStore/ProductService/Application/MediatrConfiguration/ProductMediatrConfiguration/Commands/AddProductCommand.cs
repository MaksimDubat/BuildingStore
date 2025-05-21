using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Models.RequestModels;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для добавления продукта.
    /// </summary>
    public record AddProductCommand(ProductDto Product) : IRequest<Result>;
}
