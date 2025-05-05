using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение продуктов со скидкой.
    /// </summary>
    public record GetProductsWithSaleQuery() : IRequest<IEnumerable<ProductResponseDto>>;
}
