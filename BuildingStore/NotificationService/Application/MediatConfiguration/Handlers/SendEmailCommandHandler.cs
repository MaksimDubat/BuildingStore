using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.Services;

namespace NotificationService.Application.MediatConfiguration.Handlers
{

    /// <summary>
    /// Обработчик команды отправки сообщения.
    /// </summary>
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result>
    {
        private readonly EmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public SendEmailCommandHandler(EmailSender emailSender, IUnitOfWork unitOfWork)
        {   
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.EmailMessages.GetMessageBySubjectAsync(request.Subject, cancellationToken);

            if(message is null)
            {
                return Result.Failure("Message not found");
            }

            var userEmails = await _unitOfWork.EmailsToSent.GetAllAsync(q => q, cancellationToken);

            if(userEmails is null)
            {
                return Result.Failure("Emails not found");
            }

            foreach (var email in userEmails)
            {
                await _emailSender.SendEmailAsync(email.Email, message.Subject, message.Body);
            }
            
            return Result.Success("Emails were send");
        }
    }
}
