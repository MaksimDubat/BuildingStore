using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение товара по наименованию.
    /// </summary>
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, Result<ProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductResponseDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByNameAsync(request.Name, cancellationToken);

            if (product is null)
            {
                return Result<ProductResponseDto>.Failure("Not Found");
            }

            var result = _mapper.Map<ProductResponseDto>(product);

            return Result<ProductResponseDto>.Success(result, "Product");
        }
    }
}
