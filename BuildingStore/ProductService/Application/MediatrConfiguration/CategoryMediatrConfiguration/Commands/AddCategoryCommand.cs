using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для добавления категории.
    /// </summary>
    public class AddCategoryCommand : IRequest<string>
    {
        /// <summary>
        /// Категория.
        /// </summary>
        public CategoryDto Category { get; set; }
        public AddCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
    }
}
