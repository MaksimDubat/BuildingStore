using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение товара по наименованию.
    /// </summary>
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByNameAsync(request.Name, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException("Not Found"); // что лучше null or ex
            }

            return _mapper.Map<ProductDto>(product);
        }
    }
}
