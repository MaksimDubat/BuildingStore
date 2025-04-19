using ProductService.Domain.Entities;
using QuestPDF.Fluent;

namespace ProductService.Application.Services
{
    /// <summary>
    /// Сервис по генерации PDF файлов.
    /// </summary>
    public class PdfGenerationService
    {
        public byte[] GeneratePdfForOrder(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"order #{order.OrderId}").FontSize(20).Bold();
                        col.Item().Text($"user #{order.UserId}");
                        col.Item().Text($"Status: {order.Status}");
                        col.Item().Text($"Created At: {order.CreatedAt}");
                        col.Item().Text("Items:").FontSize(16).Bold();

                        foreach (var item in order.OrderItems)
                        {
                            col.Item().Text($"• {item.Product} x{item.Amount}");
                        }

                        col.Item().Text($"Total Price: ${order.TotalPrice:F2}").Bold();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GeneratePdfForAllOrders(List<Order> orders)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Content().Column(col =>
                    {
                        col.Item().Text("All Orders").FontSize(20).Bold();

                        foreach (var order in orders)
                        {
                            col.Item().Text($"Order #{order.OrderId} - ${order.TotalPrice:F2}").Bold();
                            foreach (var item in order.OrderItems)
                            {
                                col.Item().Text($"  - {item.Product} x{item.Amount}");
                            }

                            col.Item().Text($"Total Price: ${order.TotalPrice:F2}").Bold();
                            col.Item().Text("");
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
