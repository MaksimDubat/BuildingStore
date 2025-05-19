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
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result<IEnumerable<UserResultDto>>>
    {
        private readonly IUserEmailCacheService _userEmailCache;
        private readonly EmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public SendEmailCommandHandler(IUserEmailCacheService userEmail, EmailSender emailSender, IUnitOfWork unitOfWork)
        {
            _userEmailCache = userEmail;    
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<UserResultDto>>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.EmailMessages.GetMessageBySubjectAsync(request.Subject, cancellationToken);

            if(message is null)
            {
                return Result<IEnumerable<UserResultDto>>.Failure("Message not found");
            }

            var userEmails = request.Users.ToList();

            foreach (var email in userEmails)
            {
                await _emailSender.SendEmailAsync(email.UserEmail, message.Subject, message.Body);
            }
            

            return Result<IEnumerable<UserResultDto>>.Success(userEmails, "Emails were send");
        }
    }
}
