using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Domain.Entities;

namespace ProductService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/order-managment")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Добавление в заказ одного продукта.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPost("order-single-product")]
        public async Task<ActionResult<Order>> CreateSingleProductOrder([FromBody] PurchaseSingleProductCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Order was created");
        }

        /// <summary>
        /// Заказ всех продуктов.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPost("order-all")]
        public async Task<ActionResult<Order>> CreateAllProductsOrder([FromBody] PurchaseAllProductsCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Order was created");
        }

        /// <summary>
        /// Изменение статуса заказа.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] ChangeStatusOfOrderCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Status was changed");
        }

        /// <summary>
        /// Получение всех заказов.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders(CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetAllOrdersQuery(), cancellation);
            return Ok(result);
        }

        /// <summary>
        /// Получение заказа по идентифкатору.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellation"></param>
        [HttpGet("order")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellation);
            return Ok(result);
        }
    }
}
