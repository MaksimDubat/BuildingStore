using Google.Protobuf;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Application.Services;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на формирование файла для всех заказов.
    /// </summary>
    public class GeneratePdfForAllOrdersQueryHandler : IRequestHandler<GeneratePdfForAllOrdersQuery, Result<(ByteString, string)>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PdfGenerationService _pdfGenerationService;

        public GeneratePdfForAllOrdersQueryHandler(IUnitOfWork unitOfWork, PdfGenerationService pdfGenerationService)
        {
            _unitOfWork = unitOfWork;
            _pdfGenerationService = pdfGenerationService;
        }

        public async Task<Result<(ByteString, string)>> Handle(GeneratePdfForAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(q => q, cancellationToken);

            if(orders is null)
            {
                return Result<(ByteString, string)>.Failure("Not found");
            }

            var pdfBytes = _pdfGenerationService.GeneratePdfForAllOrders(orders);

            var result = (ByteString.CopyFrom(pdfBytes), "all_orders.pdf");

            return Result<(ByteString, string)>.Success(result, "File");
        }
    }
}
