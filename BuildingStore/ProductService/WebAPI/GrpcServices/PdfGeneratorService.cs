
using Grpc.Core;
using MediatR;
using PdfGenerator.Grpc;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries;

namespace ProductService.WebAPI.GrpcServices
{
    /// <summary>
    /// Grpc сервис для работы с файлами.
    /// </summary>
    public class PdfGeneratorService : PdfGenerator.Grpc.PdfGeneratorService.PdfGeneratorServiceBase
    {
        private readonly IMediator _mediator;

        public PdfGeneratorService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<PdfResponse> GeneratePdfForOrder(OrderIdRequest request, ServerCallContext context)
        {
            var result = await _mediator.Send(new GeneratePdfForOrderQuery(request.OrderId));

            var (pdfContent, fileName) = result.Data;

            return new PdfResponse
            {
                FileName = fileName,
                PdfContent = pdfContent
            };
        }

        public override async Task<PdfResponse> GeneratePdfForAllOrders(EmptyRequest request, ServerCallContext context)
        {
            var result = await _mediator.Send(new GeneratePdfForAllOrdersQuery());

            var (pdfContent, fileName) = result.Data;

            return new PdfResponse
            {
                FileName = fileName,
                PdfContent = pdfContent
            };
        }
    }
}

