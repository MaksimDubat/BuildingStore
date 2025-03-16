using MediatR;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления категории.
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var exist = await _unitOfWork.Categories.AnyAsync(x => x.CategoryId == request.Id, cancellationToken);
            if (!exist)
            {
                return "Not found";
            }
            await _unitOfWork.Categories.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return $"Category {request.Id} was deleted";
        }
    }
}
