using MediatR;
using UserService.Application.Common;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления пользователя по идентифкатору.
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure("not found");
            }

            await _unitOfWork.Users.DeleteAsync(user.Id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Deleted");
        }
    }
}
