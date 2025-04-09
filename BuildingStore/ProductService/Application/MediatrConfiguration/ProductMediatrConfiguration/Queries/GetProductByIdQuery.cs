using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товара по идентификатору.
    /// </summary>
    public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;
}
