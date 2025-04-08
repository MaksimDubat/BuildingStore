using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления категории.
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetAsync(request.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException("Not found");
            }

            var isDuplicate = await _unitOfWork.Categories.IsCategoryExistOrDuplicateAsync(category, cancellationToken);

            if (isDuplicate)
            {
                throw new ArgumentException("Already exist");
            }

            category.CategoryName = request.CategoryName;
            
            await _unitOfWork.Categories.UpdateAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        
        }
    }
}
