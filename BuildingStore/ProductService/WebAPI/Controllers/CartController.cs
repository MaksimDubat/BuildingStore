using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Queries;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;

namespace ProductService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/cart-managment")]
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
        [HttpPost("add-product-to-cart")]
        public async Task<ActionResult<CartItemDto>> AddProductToCart([FromBody] AddProductToCartCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Product was added to the cart");
        }

        /// <summary>
        /// Создание корзины.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPost("cart")]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CreateCartCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Cart was created");
        }

        /// <summary>
        /// Удаление товара из корзины.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="cancellation"></param>
        [HttpDelete]
        public async Task<ActionResult> DeleteProductFromCart(int cartId, int productId, CancellationToken cancellation)
        {
            await _mediator.Send(new DeleteProductFromCartCommand(cartId, productId), cancellation);
            return Ok("Product was deleted");
        }

        [HttpGet("cart-items")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAllCartItems(int cartId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCartItemsQuery(cartId), cancellation);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAmountOfProducts(int cartId, int productId, int amount, CancellationToken cancellation)
        {
            await _mediator.Send(new ChangeAmountOfProductCommand(cartId, productId, amount), cancellation);
            return Ok("amount was changed");
        }
    }
}
