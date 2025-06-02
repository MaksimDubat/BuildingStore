using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.MediatrConfiguration.Handlers;
using UserService.Domain.Entities;
using UserService.Infrastructure.RefreshTokenSet;
using Xunit;

namespace UserServiceUnitTests
{
    public class RefreshTokenCommandHandlerUnitTest
    {
        private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
        private readonly Mock<IRefreshTokenGenerator> _refreshTokenGeneratorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandHandlerUnitTest()
        {
            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _refreshTokenGeneratorMock = new Mock<IRefreshTokenGenerator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            _handler = new RefreshTokenCommandHandler(
                _jwtGeneratorMock.Object,
                _refreshTokenGeneratorMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task ReturnFailureWhenRefreshNull()
        {
            // Arrange
            var command = new RefreshTokenCommand(null, "2025-01-01T00:00:00Z", new ClaimsPrincipal());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("refreshtoken not found");
        }

        [Fact]
        public async Task ReturnFailureWhenExpiresAtIsNull()
        {
            // Arrange
            var command = new RefreshTokenCommand("refresh-token", null, new ClaimsPrincipal());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("refreshtoken not found");
        }

        [Fact]
        public async Task ReturnSuccessWhenValid()
        {
            // Arrange
            var userId = 1;
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var expiresAt = DateTime.UtcNow.AddMinutes(5).ToString("O");
            var command = new RefreshTokenCommand("refresh-token", expiresAt, principal);

            var user = new AppUser { Id = userId };
            var newAccessToken = "new-access-token";
            var newRefreshToken = new RefreshToken { Refresh = "new-refresh-token", Expires = DateTime.UtcNow.AddMinutes(10) };

            _refreshTokenGeneratorMock
                .Setup(r => r.IsRefreshTokenValid(It.IsAny<RefreshToken>()))
                .Returns(true);

            _userRepositoryMock
                .Setup(u => u.GetAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _jwtGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(newAccessToken);

            _refreshTokenGeneratorMock
                .Setup(r => r.GenerateRefreshToken())
                .Returns(newRefreshToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.AccessToken.Should().Be(newAccessToken);
            result.Data.RefreshToken.Should().Be(newRefreshToken);
            result.Message.Should().Be("Tokens were refreshed");
        }
    }
}
