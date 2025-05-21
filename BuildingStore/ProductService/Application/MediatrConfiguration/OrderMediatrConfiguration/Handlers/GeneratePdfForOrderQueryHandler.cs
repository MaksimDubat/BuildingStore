using Google.Protobuf;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;
using ProductService.Application.Services;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на формирование файла для определенного заказа.
    /// </summary>
    public class GeneratePdfForOrderQueryHandler : IRequestHandler<GeneratePdfForOrderQuery, Result<(ByteString, string)>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PdfGenerationService _pdfGenerationService;

        public GeneratePdfForOrderQueryHandler(IUnitOfWork unitOfWork, PdfGenerationService pdfGenerationService)
        {
            _unitOfWork = unitOfWork;
            _pdfGenerationService = pdfGenerationService;
        }

        public async Task<Result<(ByteString, string)>> Handle(GeneratePdfForOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return Result<(ByteString, string)>.Failure("not found");
            }

            var pdfBytes = _pdfGenerationService.GeneratePdfForOrder(order);

            var result =  (ByteString.CopyFrom(pdfBytes), $"order_{order.OrderId}.pdf");

            return Result<(ByteString, string)>.Success(result, "File");
        }
    }
}
