using FluentAssertions;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.MediatrConfiguration.Handlers;
using UserService.Domain.Entities;
using Xunit;

namespace UserServiceUnitTests
{
    public class DeleteUserCommandHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();
            _unitOfWork.Setup(x => x.Users).Returns(_userRepository.Object);
            _unitOfWork.Setup(x => x.CompleteAsync(CancellationToken.None));
            _handler = new DeleteUserCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task ReturnFailureWhenInvalid()
        {
            // Arrange
            var request = new DeleteUserCommand(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("not found");
        }

        [Fact]
        public async Task ReturnSuccessWhenDeleted()
        {
            // Arrange
            var user = new AppUser { Id = 1 };

            _userRepository
                .Setup(x => x.GetAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _userRepository
                .Setup(x => x.DeleteAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWork
                .Setup(x => x.CompleteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var request = new DeleteUserCommand(user.Id);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Deleted");

            _userRepository.Verify(x => x.DeleteAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}