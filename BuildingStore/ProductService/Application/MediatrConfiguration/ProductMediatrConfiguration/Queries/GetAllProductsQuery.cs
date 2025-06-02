using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех продуктов.
    /// </summary>
    public record GetAllProductsQuery(int pageNumber, int pageSize) : IRequest<Result<IEnumerable<ProductResponseDto>>>;
}
