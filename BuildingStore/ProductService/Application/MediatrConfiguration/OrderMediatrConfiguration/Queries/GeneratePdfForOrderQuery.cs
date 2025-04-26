using Google.Protobuf;
using MediatR;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на формирование файла для определенного заказа.
    /// </summary>
    /// <param name="OrderId"></param>
    public record GeneratePdfForOrderQuery(int OrderId) : IRequest<(ByteString PdfContent, string FileName)>;
}
