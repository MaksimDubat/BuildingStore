using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Domain.Entities;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для добавления продукта.
    /// </summary>
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { CategoryName = request.CategoryName};

            if(category is null)
            {
                return Result.Failure("object is empty");
            }

            var exist = await _unitOfWork.Categories.IsCategoryExistOrDuplicateAsync(category, cancellationToken);

            if (exist)
            {
                return Result.Failure("Already exist");
            }

            await _unitOfWork.Categories.AddAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Added");
        }
    }
}
