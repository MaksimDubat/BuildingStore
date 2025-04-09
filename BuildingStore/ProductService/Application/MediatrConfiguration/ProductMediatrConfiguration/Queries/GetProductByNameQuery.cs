using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товара по наименованию.
    /// </summary>
    public record GetProductByNameQuery(string Name) : IRequest<ProductDto>;
}
