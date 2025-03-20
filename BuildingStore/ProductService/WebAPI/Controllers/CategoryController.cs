using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;

namespace ProductService.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер по работе с категориями.
    /// </summary>
    [ApiController]
    [Route("api/category-managment")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получение всех категорий.
        /// </summary>
        /// <param name="cancellation"></param>
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories(CancellationToken cancellation)
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery(), cancellation);
            return Ok(categories);
        }

        /// <summary>
        /// Получение категории по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [HttpGet("category/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id, CancellationToken cancellation)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id), cancellation);
            return Ok(category);
        }

        /// <summary>
        /// Добавление категории.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddProduct([FromBody] AddCategoryCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);
            return Ok(result);
        }

        /// <summary>
        /// Обновление категории.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellation)
        {
            var category = await _mediator.Send(command, cancellation);
            return Ok(category);
        }

        /// <summary>
        /// Удаление категории.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id), cancellation);
            return Ok("Deleted");
        }
    }
}
