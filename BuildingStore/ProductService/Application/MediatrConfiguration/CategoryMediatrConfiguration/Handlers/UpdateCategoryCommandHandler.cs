using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления категории.
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetAsync(request.Id, cancellationToken);

            if (category is null)
            {
                return Result.Failure("Not found");
            }

            var isDuplicate = await _unitOfWork.Categories.IsCategoryExistOrDuplicateAsync(category, cancellationToken);

            if (isDuplicate)
            {
                return Result.Failure("Already exist");
            }

            category.CategoryName = request.CategoryName;
            
            await _unitOfWork.Categories.UpdateAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Updated");
        
        }
    }
}
