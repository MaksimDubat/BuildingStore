using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Collections;
using NotificationService.Domain.Smtp;



namespace NotificationService.Application.Services
{
    /// <summary>
    /// Сервис по отправке email.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;
        private readonly IUnitOfWork _unitOfWork;

        public EmailSender(IOptions<SmtpSettings> settings, IUnitOfWork unitOfWork)
        {
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
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

        public async Task SendNewsletterAsync(CancellationToken cancellation)
        {
            var message = await _unitOfWork.EmailMessages.GetLatestMessageAsync(cancellation);

            var users = await _unitOfWork.EmailsToSent.GetAllAsync(q => q, cancellation);

            foreach (var user in users)
            {
                await SendEmailAsync(user.Email, message.Subject, message.Body);

                await _unitOfWork.EmailsToSent.DeleteAsync(user.Id, cancellation);
            }
        }
    }
}
