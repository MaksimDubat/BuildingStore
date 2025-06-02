using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Application.Models.RequestModels;

namespace ProductService.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер по работе с товарами.
    /// </summary>
    [ApiController]
    [Route("api/v1/product")]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllProducts([FromQuery] int page, [FromQuery] int size, 
            CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(page, size), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Получение товара по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Получение продукта по наименованию.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellation"></param>
        [HttpGet("by-name")]
        public async Task<ActionResult<ProductResponseDto>> GetProductByName([FromQuery] string name, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductByNameQuery(name), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Добавление продукта.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddProduct([FromForm] AddOrUpdateProductRequestModel model, CancellationToken cancellation)
        {
            var command = new AddProductCommand(new ProductDto
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Price = model.Price,
                Image = model.Image,
                Amount = model.Amount,
            });

            var result = await _mediator.Send(command, cancellation);

            return Ok(new { result.Message });
        }

        /// <summary>
        /// Обновление продукта.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromForm] AddOrUpdateProductRequestModel model, CancellationToken cancellation)
        {
            var command = new UpdateProductCommand(id, new ProductDto
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Price = model.Price,
                Image = model.Image,
                Amount = model.Amount,
            });

            var result = await _mediator.Send(command, cancellation);

            return Ok(new { result.Message });
        }

        /// <summary>
        /// Удаление продукта.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id), cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Фильтр для получения продуктов по категории.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("by-category")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsByCategoryName([FromQuery] string categoryName, [FromQuery] int page, 
            [FromQuery] int size, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductByCategoryQuery(categoryName, page, size), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Фильтр для получения продуктов по цене.
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="cancellation"></param>
        [HttpGet("by-price")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsByPrice([FromQuery] int page, [FromQuery] int size, decimal minPrice, decimal maxPrice, bool orderBy, CancellationToken cancellation)
        {
            var products = await _mediator.Send(new GetProductsByPriceQuery(page, size, minPrice, maxPrice, orderBy), cancellation);

            return Ok(products);
        }

        /// <summary>
        /// Фильтр для получения продуктов по дате.
        /// </summary>
        /// <param name="dayAgo"></param>
        /// <param name="orderBy"></param>
        /// <param name="cancellation"></param>
        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducsByDate([FromQuery] int page, [FromQuery] int size, int dayAgo, bool orderBy, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductsByDateQuery(page, size, dayAgo, orderBy), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Фильтр для получения доступных товаров.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAvailableProducts([FromQuery] int page, [FromQuery] int size, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductsByAmountQuery(page, size), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Добавление скидки к продукту.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPut("{id}/sale")]
        public async Task<IActionResult> AddSaleToProduct(int id, [FromBody] AddProductSaleCodeCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Получение продуктов без скидки.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("no-sale")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsWithoutSale([FromQuery] int page, [FromQuery] int size, 
            CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductsWithoutSaleQuery(page, size), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Получение продуктов со скидкой 
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsWithSale([FromQuery] int page, [FromQuery] int size,
            CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductsWithSaleQuery(page, size), cancellation);
            return Ok(new { result.Data });
        }

        /// <summary>
        /// Получение рекомендаций из формы.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellation"></param>
        [HttpGet("recomendations")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetRecomendedProducts([FromQuery] GetProductsFromFormQuery query, CancellationToken cancellation)
        {
            var result = await _mediator.Send(query,cancellation);  
            return Ok(new { result.Data });
        }
    }
}
