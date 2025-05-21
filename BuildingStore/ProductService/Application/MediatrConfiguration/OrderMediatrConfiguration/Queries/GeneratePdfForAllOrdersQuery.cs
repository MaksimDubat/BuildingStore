using Google.Protobuf;
using MediatR;
using ProductService.Application.Common;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на формирование файла для всех заказов.
    /// </summary>
    public record GeneratePdfForAllOrdersQuery() : IRequest<Result<(ByteString PdfContent, string FileName)>>;
}
