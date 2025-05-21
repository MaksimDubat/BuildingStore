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
    /// Обработчик запроса на получение товаров по новизне за опредленный период.
    /// </summary>
    public class GetProductsByDateQueryHandler : IRequestHandler<GetProductsByDateQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public GetProductsByDateQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductsByDateQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductByDateSpecification(request.DayAgo, request.OrderBy);

            if(specification is null)
            {
                return Result<IEnumerable<ProductResponseDto>>.Failure("Specification is null");
            }

            var products = await _unitOfWork.Products.GetBySpecificationAsync(specification, cancellationToken);

            if(products is null)
            {
                return Result<IEnumerable<ProductResponseDto>>.Failure("Not Found");
            }

            IEnumerable<Product> sortedProducts;

            if (request.OrderBy)
            {
                sortedProducts = products.OrderBy(x => x.Price).ApplyPagination(request.PageNumber, request.PageSize);
            }
            else
            {
                sortedProducts = products.OrderByDescending(x => x.Price).ApplyPagination(request.PageNumber, request.PageSize);
            }

            var result =  _mapper.Map<IEnumerable<ProductResponseDto>>(sortedProducts);

            return Result<IEnumerable<ProductResponseDto>>.Success(result, "Products");
        }
    }
}
