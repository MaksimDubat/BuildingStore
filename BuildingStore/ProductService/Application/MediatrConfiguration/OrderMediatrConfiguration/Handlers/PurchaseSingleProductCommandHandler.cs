using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для заказа одного товара.
    /// </summary>
    public class PurchaseSingleProductCommandHandler : IRequestHandler<PurchaseSingleProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseSingleProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PurchaseSingleProductCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _unitOfWork.CartItems.GetCartItemAsync(request.CartId, request.ProductId, cancellationToken);

            if (cartItem is null)
            {
                return Result.Failure("Cart not found");
            }

            var product = await _unitOfWork.Products.GetAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                return Result.Failure("Product not Found");
            }

            decimal finalPrice = product.Price;

            if(product.SaleCode is not null && request.SaleCode == request.SaleCode)
            {
                finalPrice = product.SalePrice ?? product.Price;
            }

            var order = new Order
            {
                UserId = request.CartId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = cartItem.Amount * finalPrice,
                OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = request.ProductId,
                            Amount = cartItem.Amount,
                            TotalPrice = cartItem.Amount * finalPrice
                        }
                    }
            };

            if(order is null)
            {
                return Result.Failure("Order is empty");
            }

            await _unitOfWork.Orders.AddAsync(order, cancellationToken);

            product.Amount -= cartItem.Amount;
            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);

            await _unitOfWork.CartItems.DeleteEntityAsync(cartItem, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Created");
        }
    }
}
