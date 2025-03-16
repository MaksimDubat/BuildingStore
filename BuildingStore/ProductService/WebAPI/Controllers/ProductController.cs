using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;

namespace ProductService.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер по работе с товарами.
    /// </summary>
    [ApiController]
    [Route("api/products-managment")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Получение всех продуктов.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetAllProductsQuery(), cancellation);
            return Ok(products);
        }
        /// <summary>
        /// Получение товара по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [HttpGet("product/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken cancellation)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id), cancellation);
            return Ok(product);
        }
        /// <summary>
        /// Получение продукта по наименованию.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellation"></param>
        [HttpGet("product-name{name}")]
        public async Task<ActionResult<ProductDto>> GetProductByName(string name, CancellationToken cancellation)
        {
            var product = await _mediator.Send(new GetProductByNameQuery(name), cancellation);
            return Ok(product);
        }
        /// <summary>
        /// Добавление продукта.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddProduct([FromBody] AddProductCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);
            return Ok(result);
        }
        /// <summary>
        /// Обновление продукта.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductCommand command, CancellationToken cancellation)
        {
            var product = await _mediator.Send(command, cancellation);
            return Ok(product);
        }
        /// <summary>
        /// Удаление продукта.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id), cancellation);
            return Ok("Deleted");
        }
    }
}
