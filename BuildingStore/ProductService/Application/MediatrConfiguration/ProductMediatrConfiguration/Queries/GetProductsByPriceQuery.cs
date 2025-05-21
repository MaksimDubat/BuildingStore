using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение продуктов отсортированных по цене.
    /// </summary>
    public record GetProductsByPriceQuery(int PageNumber, int PageSize,
        decimal MinPrice, decimal MaxPrice, bool OrderBy) : IRequest<Result<IEnumerable<ProductResponseDto>>>;

}
