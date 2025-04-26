using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса получения продуктов по наличию.
    /// </summary>
    public record GetProductsByAmountQuery() : IRequest<IEnumerable<ProductResponseDto>>;

}
