using Google.Protobuf;
using MediatR;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Application.Services;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на формирование файла для определенного заказа.
    /// </summary>
    public class GeneratePdfForOrderQueryHandler : IRequestHandler<GeneratePdfForOrderQuery, (ByteString, string)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PdfGenerationService _pdfGenerationService;

        public GeneratePdfForOrderQueryHandler(IUnitOfWork unitOfWork, PdfGenerationService pdfGenerationService)
        {
            _unitOfWork = unitOfWork;
            _pdfGenerationService = pdfGenerationService;
        }

        public async Task<(ByteString, string)> Handle(GeneratePdfForOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new ArgumentNullException("not found");
            }

            var pdfBytes = _pdfGenerationService.GeneratePdfForOrder(order);

            return (ByteString.CopyFrom(pdfBytes), $"order_{order.OrderId}.pdf");
        }
    }
}
