using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Infrastructure.Specifications;

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
            await _mediator.Send(command, cancellation);
            return Ok("Roduct was added");
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
            await _mediator.Send(command, cancellation);
            return Ok("Product was updated");
        }

        /// <summary>
        /// Удаление продукта.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id, CancellationToken cancellation)
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellation);
            return Ok("Deleted");
        }

        /// <summary>
        /// Фильтр для получения продуктов по категории.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("productsbycategory/{categoryName}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryName(string categoryName,  CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductByCategoryQuery(categoryName), cancellation);
            return Ok(products);
        }

        /// <summary>
        /// Фильтр для получения продуктов по цене.
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="cancellation"></param>
        [HttpGet("productsbyprice")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByPrice(decimal minPrice, decimal maxPrice, bool orderBy, CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductsByPriceQuery(minPrice, maxPrice, orderBy), cancellation);
            return Ok(products);
        }

        /// <summary>
        /// Фильтр для получения продуктов по дате.
        /// </summary>
        /// <param name="dayAgo"></param>
        /// <param name="orderBy"></param>
        /// <param name="cancellation"></param>
        [HttpGet("productsbydate")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducsByDate(int dayAgo, bool orderBy, CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductsByDateQuery(dayAgo, orderBy), cancellation);
            return Ok(products);
        }

        /// <summary>
        /// Фильтр для получения доступных товаров.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("availableproducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAvailableProducts(CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductsByAmountQuery(), cancellation);
            return Ok(products);
        }

        /// <summary>
        /// Добавление скидки к продукту.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPut("sale-managment")]
        public async Task<IActionResult> AddSaleToProduct([FromBody] AddProductSaleCodeCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Added");
        }

        /// <summary>
        /// Получение продуктов без скидки.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("nosale")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsWithoutSale(CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductsWithoutSaleQuery(), cancellation);
            return Ok(products);
        }

        /// <summary>
        /// Получение продуктов со скидкой 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsWithSale(CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductsWithSaleQuery(), cancellation);
            return Ok(products);
        }
    }
}
