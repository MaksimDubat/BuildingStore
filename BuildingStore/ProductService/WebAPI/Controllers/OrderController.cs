using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfGenerator.Grpc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Domain.Entities;

namespace ProductService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/order")]
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
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("{cartId}/order/{productId}")]
        public async Task<ActionResult<Order>> CreateSingleProductOrder(int cartId, int productId, [FromQuery] string saleCode,
            CancellationToken cancellation)
        {
            await _mediator.Send(new PurchaseSingleProductCommand(cartId, productId, saleCode), cancellation);
            return Ok("Order was created");
        }

        /// <summary>
        /// Заказ всех продуктов.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "ManagerAdminUserPolicy")]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<Order>> CreateAllProductsOrder(int cartId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new PurchaseAllProductsCommand(cartId), cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Изменение статуса заказа.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPut("status")]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] ChangeStatusOfOrderCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Получение всех заказов.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders([FromQuery] int page, [FromQuery] int size, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetAllOrdersQuery(page, size), cancellation);
            return Ok(result);
        }

        /// <summary>
        /// Получение заказа по идентифкатору.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Формирование документа определнного заказа.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("file")]
        public async Task<PdfResponse> GetPdfFileOrder([FromQuery] OrderIdRequest request, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GeneratePdfForOrderQuery(request.OrderId), cancellation);
            return new PdfResponse
            {
                PdfContent = result.Data.PdfContent,
                FileName = result.Data.FileName
            };
        }

        /// <summary>
        /// Формирование документов для всех заказов.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("files")]
        public async Task<PdfResponse> GetPdfFileOrders([FromQuery] EmptyRequest request, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GeneratePdfForAllOrdersQuery(), cancellation);
            return new PdfResponse
            {
                PdfContent = result.Data.PdfContent,
                FileName = result.Data.FileName
            };
        }

        /// <summary>
        /// Скачивание файла для всех заказов.
        /// </summary>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("downloadfiles")]
        public async Task<IActionResult> DownloadPdfForAllOrders(CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GeneratePdfForAllOrdersQuery(), cancellation);

            var (pdfContent, fileName) = result.Data;

            return File(
                pdfContent.ToByteArray(),          
                "application/pdf",             
                fileName                           
            );
        }

        /// <summary>
        /// Скачивание файла для опрделенного заказа.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("downloadfile")]
        public async Task<IActionResult> DownloadPdfForOrder([FromQuery] OrderIdRequest request, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GeneratePdfForOrderQuery(request.OrderId), cancellation);

            var (pdfContent, fileName) = result.Data;

            return File(
                pdfContent.ToByteArray(),
                "application/pdf",
                fileName
            );
        }
    }
}
