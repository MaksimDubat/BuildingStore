using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления категории.
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetAsync(request.Id, cancellationToken);
            if (category == null)
            {
                return "Category not found";
            }
            var isDuplicate = await _unitOfWork.Categories.AnyAsync(x => x.CategoryId != request.Id &&
            x.CategoryName == request.Category.CategoryName, cancellationToken);
            if (isDuplicate)
            {
                return "Duplicate";
            }
            category.CategoryName = request.Category.CategoryName;
            
            await _unitOfWork.Categories.UpdateAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return "Category was updated";
        }
    }
}
