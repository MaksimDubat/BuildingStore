using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса для получения всех товаров в корзине.
    /// </summary>
    public record GetCartItemsQuery(int CartId) : IRequest<Result<IEnumerable<CartItemDto>>>;

}
