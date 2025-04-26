using Google.Protobuf;
using MediatR;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на формирование файла для всех заказов.
    /// </summary>
    public record GeneratePdfForAllOrdersQuery() : IRequest<(ByteString PdfContent, string FileName)>;
}
