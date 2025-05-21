using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Domain.Entities;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на добавление товара в коризну.
    /// </summary>
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductToCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.Carts.GetAsync(request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure("cart wasnt found");
            }

            var product = await _unitOfWork.Products.GetAsync(request.ProductId, cancellationToken);

            if(product is null)
            {
                return Result.Failure("product wasnt found");
            }
            
            if(request.Amount > product.Amount)
            {
                return Result.Failure("such amount is not available now");
            }

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = product.ProductId,
                ProductName = product.Name,
                Amount = request.Amount,
            };


            await _unitOfWork.CartItems.AddAsync(cartItem, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Added");
        }
    }
}
