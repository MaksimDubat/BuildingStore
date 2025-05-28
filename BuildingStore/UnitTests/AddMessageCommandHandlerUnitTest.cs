using FluentAssertions;
using Moq;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Domain.Collections;
using Xunit;

namespace UnitTests
{
    public class AddMessageCommandHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly AddMessageCommandHandler _handler;
        private readonly Mock<IEmailMessageRepository> _emailMessageRepository;

        public AddMessageCommandHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _emailMessageRepository = new Mock<IEmailMessageRepository>();
            _unitOfWork.Setup(x => x.EmailMessages).Returns(_emailMessageRepository.Object);
            _handler = new AddMessageCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task ReturnFailuerWhenNull()
        {
            // Arrange
            var request = new AddMessageCommand(new MessageModel { Subject = null, Body = null });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Message is empty");
        }

        [Fact]
        public async Task ReturnSuccessWhendAdded()
        {
            // Arrange
            var dto = new MessageModel { Subject = "Test", Body = "Test" };
            var request = new AddMessageCommand(dto);

            _emailMessageRepository
                .Setup(x => x.AddEntityAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Added");
            result.Data.Subject.Should().Be("Test");
            result.Data.Body.Should().Be("Test");

            _emailMessageRepository.Verify(x => x.AddEntityAsync(It.Is<EmailMessage>(x => x.Subject == "Test" && x.Body == "Test"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}