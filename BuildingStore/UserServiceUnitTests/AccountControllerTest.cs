using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using System.Threading;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Infrastructure.RefreshTokenSet;
using UserService.WebAPI.Controllers;
using Xunit;

namespace UserServiceUnitTests
{
    public class AccountControllerTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IUserProfileCacheService> _cacheService;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _cacheService = new Mock<IUserProfileCacheService>();
            _controller = new AccountController(_mediator.Object, _cacheService.Object);
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task ReturnOkRegistration()
        {
            // Arrange
            var registrationModel = new RegistrationModel { };
            var userDto = new UserDto { Id = 1 };
            var expectedMessage = "Registration successful";

            var result = Result<UserDto>.Success(userDto, expectedMessage);

            _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.Register(registrationModel, CancellationToken.None);

            // Assert
            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var message = doc.RootElement.GetProperty("Message").GetString();

            message.Should().Be(expectedMessage);

            _cacheService.Verify(c =>
                c.SetProfileAsync(userDto.Id, userDto, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]  
        public async Task ReturnOkLogin()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedUser = new UserDto { Id = 1 };
            var expectedRefreshToken = new RefreshToken
            {
                Refresh = "refresh-token",
                Expires = DateTime.UtcNow.AddHours(1)
            };

            var expectedResult = new LoginResult
            {
                User = new AppUser { Id = 1 },
                AccessToken = "access-token",
                RefreshToken = expectedRefreshToken
            };

            var expectedMessage = "Login successful";
            var result = Result<LoginResult>.Success(expectedResult, expectedMessage);

            _mediator.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            _cacheService.Setup(c => c.GetOrSetProfileAsync(expectedUser.Id, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUser);

            // Act
            var response = await _controller.Login(loginModel, CancellationToken.None);

            // Assert
            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var message = doc.RootElement.GetProperty("Message").GetString();

            message.Should().Be(expectedMessage);

            _cacheService.Verify(c =>
                c.GetOrSetProfileAsync(expectedUser.Id, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()),
                Times.Once);

            var setCookieHeader = _controller.Response.Headers["Set-Cookie"].ToString();
            setCookieHeader.Should().Contain("cookies");
            setCookieHeader.Should().Contain("fresh-cookies");
            setCookieHeader.Should().Contain("fresh-time-cookies");
        }
    }
}
