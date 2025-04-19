using Google.Protobuf;
using MediatR;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    public record GeneratePdfForOrderQuery(int OrderId) : IRequest<(ByteString PdfContent, string FileName)>;
}
