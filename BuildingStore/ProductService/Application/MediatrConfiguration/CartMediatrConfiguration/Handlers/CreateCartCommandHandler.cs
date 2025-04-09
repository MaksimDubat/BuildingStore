using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = new Cart
            {
                CartId = request.UserId,
                UserId = request.UserId,
            };

            if(cart == null)
            {
                throw new ArgumentNullException("request is null");
            }

            await _unitOfWork.Carts.AddAsync(cart, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
