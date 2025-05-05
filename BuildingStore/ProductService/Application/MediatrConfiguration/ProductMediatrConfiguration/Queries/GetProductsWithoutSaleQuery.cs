using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех товаров без скидок.
    /// </summary>
    public record GetProductsWithoutSaleQuery() : IRequest<IEnumerable<ProductResponseDto>>;

}
