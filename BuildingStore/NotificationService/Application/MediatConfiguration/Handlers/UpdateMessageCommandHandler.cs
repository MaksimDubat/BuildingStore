using MediatR;
using MongoDB.Bson;
using NotificationService.Application.Common;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;

namespace NotificationService.Application.MediatConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления сообщения.
    /// </summary>
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(request.MessageId, out ObjectId messageId))
            {
                return Result.Failure("Invalid message ID format");
            }

            var message = await _unitOfWork.EmailMessages.GetByIdAsync(messageId, cancellationToken);

            if (message is null)
            {
                return Result.Failure("Not found");
            }

            message.Subject = request.Message.Subject;
            message.Body = request.Message.Body;

            await _unitOfWork.EmailMessages.UpdateAsync(message, cancellationToken);

            return Result.Success("Updated");
        }
    }
}
