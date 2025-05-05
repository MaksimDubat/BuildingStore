using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех продуктов.
    /// </summary>
    public record GetAllProductsQuery : IRequest<IEnumerable<ProductResponseDto>>;
}
