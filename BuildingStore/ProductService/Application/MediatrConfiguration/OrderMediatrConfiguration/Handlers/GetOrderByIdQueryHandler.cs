using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение заказа по идентифкатору.
    /// </summary>
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Orders.GetOrderByIdAsync(request.OrderId, cancellationToken);

            if (result == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            return _mapper.Map<OrderDto>(result);
        }
    }
}
