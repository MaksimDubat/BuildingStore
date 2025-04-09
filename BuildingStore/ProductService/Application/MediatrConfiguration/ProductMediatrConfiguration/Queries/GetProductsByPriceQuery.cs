using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение продуктов отсортированных по цене.
    /// </summary>
    public record GetProductsByPriceQuery(decimal MinPrice, decimal MaxPrice, bool OrderBy) : IRequest<IEnumerable<ProductDto>>;

}
