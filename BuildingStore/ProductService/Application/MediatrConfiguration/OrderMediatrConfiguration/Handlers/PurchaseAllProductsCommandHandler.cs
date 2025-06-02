using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на заказ всех продуктов из корзины.
    /// </summary>
    public class PurchaseAllProductsCommandHandler : IRequestHandler<PurchaseAllProductsCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseAllProductsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PurchaseAllProductsCommand request, CancellationToken cancellationToken)
        {
            var carItems = await _unitOfWork.CartItems.GetCartItemsAsync(request.CartId, cancellationToken);

            if(carItems is null)
            {
                return Result.Failure("Cart items are empty");
            }

            var order = new Order
            {
                UserId = request.CartId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = 0,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in carItems)
            {
                var product = await _unitOfWork.Products.GetAsync(item.ProductId, cancellationToken);

                if (product is null)
                {
                    return Result.Failure("Product not found");
                }

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Amount = item.Amount,
                    TotalPrice = item.Amount * product.Price
                };

                if(orderItem is null)
                {
                    return Result.Failure("Order items are empty");
                }
               
                order.OrderItems.Add(orderItem);
                order.TotalPrice += orderItem.TotalPrice;

                product.Amount -= item.Amount;
                await _unitOfWork.Products.UpdateAsync(product, cancellationToken);

                await _unitOfWork.CartItems.DeleteEntityAsync(item, cancellationToken);
            }

            await _unitOfWork.Orders.AddAsync(order, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Order was created");
        }
    }
}
