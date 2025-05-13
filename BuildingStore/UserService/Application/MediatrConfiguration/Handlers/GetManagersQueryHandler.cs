using AutoMapper;
using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Extensions;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Domain.Enums;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение менеджеров.
    /// </summary>
    public class GetManagersQueryHandler : IRequestHandler<GetManagersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetManagersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetManagersQuery request, CancellationToken cancellationToken)
        {
            var managers = await _unitOfWork.Users.GetAllAsync(q => q.Where(x => x.Role == UserRole.Manager).ApplyPagination(request.PazeNumber, request.PageSize), cancellationToken);

            var result = _mapper.Map<IEnumerable<UserDto>>(managers);

            return Result<IEnumerable<UserDto>>.Success(result, "Users");
        }
    }
}
