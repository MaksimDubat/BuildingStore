using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.MediatrConfiguration.Handlers;
using Xunit;

namespace UserServiceUnitTests
{
    public class LogoutCommandHandlerUnitTest
    {
        private readonly LogoutCommandHandler _handler;

        public LogoutCommandHandlerUnitTest()
        {
            _handler = new LogoutCommandHandler();
        }

        [Fact]
        public async Task ReturnSuccsesWhenValid()
        {
            // Arrange
            var userId = 42;
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var command = new LogoutCommand(principal);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(userId);
            result.Message.Should().Be("Bye");
        }

        [Fact]  
        public async Task ReturnFailureWhenClaimIsNull()
        {
            // Arrange
            var identity = new ClaimsIdentity(); // no claims
            var principal = new ClaimsPrincipal(identity);
            var command = new LogoutCommand(principal);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Invalid user ID.");
        }

        [Fact]
        public async Task ReturnFailureWhenClaimIsInvalid()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "not-a-number") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var command = new LogoutCommand(principal);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Invalid user ID.");
        }
    }
}
