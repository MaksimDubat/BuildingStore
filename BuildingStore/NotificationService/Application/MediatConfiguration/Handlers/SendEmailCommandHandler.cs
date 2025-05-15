using MediatR;
using NotificationService.Application.Common;
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
        private readonly IUserServiceClient _userServiceClient;
        private readonly EmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public SendEmailCommandHandler(IUserServiceClient userServiceClient, EmailSender emailSender, IUnitOfWork unitOfWork)
        {
            _userServiceClient = userServiceClient;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var userEmails = await _userServiceClient.GetAllUserEmailsAsync(cancellationToken);

            if(userEmails is not { Count: > 0})
            {
                return Result.Failure("emails not found");
            }

            var message = await _unitOfWork.EmailMessages.GetMessageBySubjectAsync(request.Subject, cancellationToken);

            if(message is null)
            {
                return Result.Failure("Message not found");
            }

            foreach (var email in userEmails)
            {
                await _emailSender.SendEmailAsync(email, message.Subject, message.Body);
            }

            return Result.Success("Emails was sended");
        }
    }
}
