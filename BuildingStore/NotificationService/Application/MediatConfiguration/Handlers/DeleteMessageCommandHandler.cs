using MediatR;
using MongoDB.Bson;
using NotificationService.Application.Common;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;

namespace NotificationService.Application.MediatConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на удаление сообщения.
    /// </summary>
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(request.MessageId, out ObjectId messageId))
            {
                return Result.Failure("Invalid message ID format");
            }

            var message = await _unitOfWork.EmailMessages.GetByIdAsync(messageId, cancellationToken);

            if (message is null)
            {
                return Result.Failure("Message not found");
            }

            await _unitOfWork.EmailMessages.DeleteAsync(message.Id, cancellationToken);

            return Result.Success("Deleted");
        }
    }
}
