using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления категории.
    /// </summary>
    public class UpdateCategoryCommand : IRequest<string>
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Категория.
        /// </summary>
        public CategoryDto Category { get; set; }
        public UpdateCategoryCommand(int id, CategoryDto category)
        {
            Id = id;
            Category = category;
        }
    }
}
