using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Domain.Collections;


namespace NotificationService.Application.MediatConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для добавления сообщений.
    /// </summary>
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result<EmailMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EmailMessageDto>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            if(request.Model.Subject is null || request.Model.Body is null)
            {
                return Result<EmailMessageDto>.Failure("Message is empty");
            }

            var message = new EmailMessage
            {
                Subject = request.Model.Subject,
                Body = request.Model.Body,
            };

            await _unitOfWork.EmailMessages.AddEntityAsync(message, cancellationToken);

            var result = new EmailMessageDto
            {
                Subject = message.Subject,
                Body = message.Body,
            };

            return Result<EmailMessageDto>.Success(result, "Added");
        }
    }
}
