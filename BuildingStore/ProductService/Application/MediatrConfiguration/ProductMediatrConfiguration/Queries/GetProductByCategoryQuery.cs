using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товаров по наименованию категории.
    /// </summary>
    public record GetProductByCategoryQuery(string Name, int PageNumber, int PageSize) : IRequest<Result<IEnumerable<ProductResponseDto>>>;

}
