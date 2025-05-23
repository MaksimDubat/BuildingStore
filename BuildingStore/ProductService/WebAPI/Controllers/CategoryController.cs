using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries;

namespace ProductService.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер по работе с категориями.
    /// </summary>
    [ApiController]
    [Route("api/v1/category")]
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
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories([FromQuery] int page, [FromQuery] int size, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(page, size), cancellation);

            return Ok(new { result.Data });
        }

        /// <summary>
        /// Получение категории по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellation);

            return Ok(new { result.Data });
        }

        /// <summary>
        /// Добавление категории.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] AddCategoryCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);

            return Ok(new { result.Message });
        }

        /// <summary>
        /// Обновление категории.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);
            return Ok(new { result.Message });
        }

        /// <summary>
        /// Удаление категории.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id), cancellation);

            return Ok(new { result.Message });
        }
    }
}
