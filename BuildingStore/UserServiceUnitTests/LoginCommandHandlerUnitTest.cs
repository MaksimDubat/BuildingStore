using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.MediatrConfiguration.Handlers;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Infrastructure.RefreshTokenSet;
using Xunit;

namespace UserServiceUnitTests
{
    public class LoginCommandHandlerUnitTest
    {
        private readonly Mock<IAuthenticationService> _authService;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IRefreshTokenGenerator> _refreshTokenGenerator;
        private readonly Mock<IUserProfileCacheService> _profileCacheService;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerUnitTest()
        {
            _authService = new Mock<IAuthenticationService>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();
            _refreshTokenGenerator = new Mock<IRefreshTokenGenerator>();
            _profileCacheService = new Mock<IUserProfileCacheService>();
            _unitOfWork.Setup(u => u.Users).Returns(_userRepository.Object);
            _handler = new LoginCommandHandler(_authService.Object, _unitOfWork.Object, _mapper.Object, _refreshTokenGenerator.Object);
        }

        [Fact]
        public async Task ReturnWhenTokenValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var accessToken = "valid-token";
            var refreshToken = new RefreshToken { Refresh = "refresh-token", Expires = DateTime.UtcNow.AddMinutes(5) };

            var user = new AppUser { Id = 1, UserEmail = email };
            var command = new LoginCommand(new LoginModel { Email = email, Password = password });

            _authService.Setup(a => a.SignInAsync(email, password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(accessToken);

            _userRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _refreshTokenGenerator.Setup(r => r.GenerateRefreshToken())
                .Returns(refreshToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.AccessToken.Should().Be(accessToken);
            result.Data.RefreshToken.Should().Be(refreshToken);
            result.Data.User.Should().Be(user);
            result.Message.Should().Be("Hello");
        }

        [Fact]
        public async Task ReturnFailureWhenTokenIsNull()
        {
            // Assert
            var email = "test@test.com";
            var password = "test";

            var model = new LoginModel { Email = email, Password = password };
            var command = new LoginCommand(model);

            _authService
                .Setup(x => x.SignInAsync(email, password, It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("operation invalid");
        }

        [Fact]
        public async Task ReturnFailureWhenUserNotFound()
        {
            // Arrange
            var email = "test@test.com";
            var password = "password123";
            var accessToken = "token";

            var model = new LoginModel { Email = email, Password = password };
            var command = new LoginCommand(model);

            _authService.Setup(a => a.SignInAsync(email, password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(accessToken);

            _userRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((AppUser)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }
    }
}
