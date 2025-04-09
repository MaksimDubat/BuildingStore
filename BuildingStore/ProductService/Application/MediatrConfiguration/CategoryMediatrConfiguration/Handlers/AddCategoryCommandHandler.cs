using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для добавления продукта.
    /// </summary>
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { CategoryName = request.CategoryName};

            if(category == null)
            {
                throw new ArgumentNullException("object is empty");
            }

            var exist = await _unitOfWork.Categories.IsCategoryExistOrDuplicateAsync(category, cancellationToken);

            if (exist)
            {
                throw new ArgumentException("Already exist");
            }

            await _unitOfWork.Categories.AddAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
