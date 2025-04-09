using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для добавления категории.
    /// </summary>
    public class AddCategoryCommand : IRequest
    {
        /// <summary>
        /// Категория.
        /// </summary>
        public string CategoryName { get; set; }

        public AddCategoryCommand(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
