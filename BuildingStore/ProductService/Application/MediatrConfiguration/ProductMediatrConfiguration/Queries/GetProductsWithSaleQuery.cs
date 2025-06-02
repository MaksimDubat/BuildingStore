using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение продуктов со скидкой.
    /// </summary>
    public record GetProductsWithSaleQuery(int PageNumber, int PageSize) : IRequest<Result<IEnumerable<ProductResponseDto>>>;
}
