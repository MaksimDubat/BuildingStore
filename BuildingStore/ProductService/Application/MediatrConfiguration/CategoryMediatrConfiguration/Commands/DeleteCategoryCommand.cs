using MediatR;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для удаления категории.
    /// </summary>
    public class DeleteCategoryCommand : IRequest
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int Id { get; set; }

        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
