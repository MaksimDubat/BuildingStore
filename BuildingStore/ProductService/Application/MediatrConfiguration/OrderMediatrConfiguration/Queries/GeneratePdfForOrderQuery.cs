using Google.Protobuf;
using MediatR;
using ProductService.Application.Common;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на формирование файла для определенного заказа.
    /// </summary>
    /// <param name="OrderId"></param>
    public record GeneratePdfForOrderQuery(int OrderId) : IRequest<Result<(ByteString PdfContent, string FileName)>>;
}
