namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для отправки сообщений.
    /// </summary>
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);

        Task SendNewsletterAsync(CancellationToken cancellation);
    }
}
