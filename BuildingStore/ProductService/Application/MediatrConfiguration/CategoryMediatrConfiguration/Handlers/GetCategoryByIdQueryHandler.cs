using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries;
using ProductService.Domain.Entities;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение категории по идентификатору.
    /// </summary>
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetAsync(request.Id, cancellationToken);

            if (category is null)
            {
                return Result<CategoryDto>.Failure("Not found");
            }

            var result = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.Success(result, "Category");
        }
    }
}
