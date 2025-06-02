using MediatR;
using MongoDB.Bson;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Queries;

namespace NotificationService.Application.MediatConfiguration.Handlers
{
    /// <summary>
    /// запроса на получение сообщения по идентифкатору.
    /// </summary>
    public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, Result<EmailMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<EmailMessageDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(request.MessageId, out ObjectId messageId))
            {
                return Result<EmailMessageDto>.Failure("Invalid message ID format");
            }

            var message = await _unitOfWork.EmailMessages.GetByIdAsync(messageId, cancellationToken);

            if (message is null)
            {
                return Result<EmailMessageDto>.Failure("not found");
            }

            var result = new EmailMessageDto
            {
                Id = message.Id.ToString(),
                Subject = message.Subject,
                Body = message.Body,
            };

            return Result<EmailMessageDto>.Success(result, "Message");
        }
    }
}
