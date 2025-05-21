using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Orders.GetAllAsync(q => q.Include(x => x.OrderItems)
                .ApplyPagination(request.PageNumber, request.PageSize), cancellationToken);

            if(result == null)
            {
                throw new ArgumentNullException("No active orders");
            }

            return _mapper.Map<IEnumerable<OrderDto>>(result);
        }
    }
}
