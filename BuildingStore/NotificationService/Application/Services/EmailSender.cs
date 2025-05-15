using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Domain.Smtp;



namespace NotificationService.Application.Services
{
    /// <summary>
    /// Сервис по отправке email.
    /// </summary>
    public class EmailSender
    {
        private readonly SmtpSettings _settings;

        public EmailSender(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value; 
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = body
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, _settings.UseSsl);

            await client.AuthenticateAsync(_settings.Username, _settings.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
