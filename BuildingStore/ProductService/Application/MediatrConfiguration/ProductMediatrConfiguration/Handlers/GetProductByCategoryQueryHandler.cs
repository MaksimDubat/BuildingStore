using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Infrastructure.Specifications;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение товаров по наименованию категории.
    /// </summary>
    public class GetProductByCategoryQueryHandler : IRequestHandler<GetProductByCategoryQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductByCategorySpecification(request.Name);

            if(specification is null)
            {
                return Result<IEnumerable<ProductResponseDto>>.Failure("Specification is null");
            }

            var products = await _unitOfWork.Products.GetBySpecificationAsync(specification, cancellationToken);

            if (products is null)
            {
               return Result<IEnumerable<ProductResponseDto>>.Failure("Not Found");
            }

            var paginatedProducts = products.ApplyPagination(request.PageNumber, request.PageSize);

            var result = _mapper.Map<IEnumerable<ProductResponseDto>>(paginatedProducts);

            return Result<IEnumerable<ProductResponseDto>>.Success(result, "Products");
        }
    }
}
