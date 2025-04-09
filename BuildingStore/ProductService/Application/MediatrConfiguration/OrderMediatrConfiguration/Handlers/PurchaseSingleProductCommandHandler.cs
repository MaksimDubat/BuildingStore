using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для заказа одного товара.
    /// </summary>
    public class PurchaseSingleProductCommandHandler : IRequestHandler<PurchaseSingleProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseSingleProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(PurchaseSingleProductCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _unitOfWork.CartItems.GetCartItemAsync(request.CartId, request.ProductId, cancellationToken);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart not found");
            }

            var product = await _unitOfWork.Products.GetAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not Found");
            }

            decimal finalPrice = product.Price;

            if(product.SaleCode != null && request.SaleCode == request.SaleCode)
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

            if(order == null)
            {
                throw new ArgumentNullException("Order is empty");
            }

            await _unitOfWork.Orders.AddAsync(order, cancellationToken);

            product.Amount -= cartItem.Amount;
            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);

            await _unitOfWork.CartItems.DeleteCartItemAsync(cartItem.CartId, cartItem.ProductId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
