using MediatR;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления категории.
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetAsync(request.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException("Not found");
            }

            await _unitOfWork.Categories.DeleteAsync(category.CategoryId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
