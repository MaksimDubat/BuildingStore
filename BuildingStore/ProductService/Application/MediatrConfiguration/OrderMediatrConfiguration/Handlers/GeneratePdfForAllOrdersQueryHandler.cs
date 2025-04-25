using Google.Protobuf;
using MediatR;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Application.Services;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на формирование файла для всех заказов.
    /// </summary>
    public class GeneratePdfForAllOrdersQueryHandler : IRequestHandler<GeneratePdfForAllOrdersQuery, (ByteString, string)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PdfGenerationService _pdfGenerationService;

        public GeneratePdfForAllOrdersQueryHandler(IUnitOfWork unitOfWork, PdfGenerationService pdfGenerationService)
        {
            _unitOfWork = unitOfWork;
            _pdfGenerationService = pdfGenerationService;
        }

        public async Task<(ByteString, string)> Handle(GeneratePdfForAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(cancellationToken);

            if(orders == null)
            {
                throw new ArgumentNullException("Not found");
            }

            var pdfBytes = _pdfGenerationService.GeneratePdfForAllOrders(orders);

            return (ByteString.CopyFrom(pdfBytes), "all_orders.pdf");
        }
    }
}
