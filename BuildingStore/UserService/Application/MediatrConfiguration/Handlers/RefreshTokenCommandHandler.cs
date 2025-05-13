using MediatR;
using System.Security.Claims;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления токенов.
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokensResult>>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(IJwtGenerator jwtGenerator, IRefreshTokenGenerator refreshTokenGenerator,
             IUnitOfWork unitOfWork)
        {
            _jwtGenerator = jwtGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokensResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.Refresh is null)
            {
                return Result<RefreshTokensResult>.Failure("refreshtoken not found");
            }

            if (request.ExpiresAtString is null)
            {
                return Result<RefreshTokensResult>.Failure("refreshtoken not found");
            }

            var expires = DateTime.Parse(request.ExpiresAtString, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var refreshToken = new RefreshToken
            {
                Refresh = request.Refresh,
                Expires = expires
            };

            var isValid = _refreshTokenGenerator.IsRefreshTokenValid(refreshToken);

            if (!isValid)
            {
                return Result<RefreshTokensResult>.Failure("refreshtoken is invalid");
            }

            var userFromCoockie = request.User;

            if (userFromCoockie is null)
            {
                return Result<RefreshTokensResult>.Failure("user not found");
            }

            var userIdClaim = userFromCoockie.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                return Result<RefreshTokensResult>.Failure("user not found");
            }

            var result = int.TryParse(userIdClaim.Value, out var userId);

            if (!result)
            {
                return Result<RefreshTokensResult>.Failure("wrong operation");
            }

            var user = await _unitOfWork.Users.GetAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result<RefreshTokensResult>.Failure("user not found");
            }

            var newAccessToken = _jwtGenerator.GenerateToken(user);

            if(newAccessToken is null)
            {
                return Result <RefreshTokensResult>.Failure("wrong generation");
            }

            var newRefreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            if (newRefreshToken is null)
            {
                return Result<RefreshTokensResult>.Failure("wrong generation");
            }

            var refreshTokensResult = new RefreshTokensResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return Result<RefreshTokensResult>.Success(refreshTokensResult, "Tokens were refreshed");

        }
    }
}
