using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Queries;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;

namespace ProductService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/cart")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Добавление товара в коризну.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult<CartItemDto>> AddProductToCart(int cartId, [FromBody] AddProductToCartCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Удаление товара из корзины.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpDelete("{cartId}/items/{productId}")]
        public async Task<ActionResult> DeleteProductFromCart(int cartId, int productId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteProductFromCartCommand(cartId, productId), cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Получение всех товаров из корзины.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpGet("{cartId}/items")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAllCartItems(int cartId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCartItemsQuery(cartId), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Измненение количества товаров в корзине.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="amount"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPut("{cartId}/items/{productId}")]
        public async Task<ActionResult> UpdateAmountOfProducts(int cartId, int productId, [FromQuery] int amount, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new ChangeAmountOfProductCommand(cartId, productId, amount), cancellation);
            return Ok(new { result.Message });
        }
    }
}
