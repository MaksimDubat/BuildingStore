using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Specifications;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса для получения продуктов по наличию.
    /// </summary>
    public class GetProductsByAmountQueryHandler : IRequestHandler<GetProductsByAmountQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByAmountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductsByAmountQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductByAmountSpecification();

            var products = await _unitOfWork.Products.GetBySpecificationAsync(specification, cancellationToken);

            if (products is null)
            {
               return Result<IEnumerable<ProductResponseDto>>.Failure("Not Found");
            }

            var paginatedProducts = products.ApplyPagination(request.PageNumber, request.PageSize);

            var result =  _mapper.Map<IEnumerable<ProductResponseDto>>(paginatedProducts);

            return Result<IEnumerable<ProductResponseDto>>.Success(result, "Products");
        }
    }
}
