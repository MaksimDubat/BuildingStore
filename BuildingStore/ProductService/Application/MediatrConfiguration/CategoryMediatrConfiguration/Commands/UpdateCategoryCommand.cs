using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для обновления категории.
    /// </summary>
    public class UpdateCategoryCommand : IRequest<Result>
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Категория.
        /// </summary>
        public string CategoryName { get; set; }

        public UpdateCategoryCommand(int id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName;
        }
    }
}
