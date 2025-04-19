using Google.Protobuf;
using MediatR;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    public record GeneratePdfForAllOrdersQuery() : IRequest<(ByteString PdfContent, string FileName)>;
}
