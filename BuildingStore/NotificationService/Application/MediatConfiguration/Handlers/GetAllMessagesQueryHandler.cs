using MediatR;
using NotificationService.Application.Common;
using NotificationService.Application.DTOs;
using NotificationService.Application.Extensions;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Queries;
using UserService.Application.Extensions;

namespace NotificationService.Application.MediatConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение всех сообщений.
    /// </summary>
    public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, Result<IEnumerable<EmailMessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<EmailMessageDto>>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _unitOfWork.EmailMessages.GetAllAsync(q => q.ApplyPagination(request.PageNumber, request.PageSize),
                cancellationToken);

            var result = messages.Select(e => new EmailMessageDto
            {
                Id = e.Id.ToString(),
                Subject = e.Subject,
                Body = e.Body,
            });

            return Result<IEnumerable<EmailMessageDto>>.Success(result, "Messages");
        }
    }
}
