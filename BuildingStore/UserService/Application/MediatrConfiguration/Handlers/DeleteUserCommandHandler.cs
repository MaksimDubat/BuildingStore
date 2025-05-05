using MediatR;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления пользователя по идентифкатору.
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("not found");
            }

            await _unitOfWork.Users.DeleteAsync(user.Id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
