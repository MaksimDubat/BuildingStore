using FluentAssertions;
using MongoDB.Bson;
using Moq;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Domain.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NotificationServiceUnitTests
{
    public class DeleteMessageCommandHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly DeleteMessageCommandHandler _handler;
        private readonly Mock<IEmailMessageRepository> _emailMessageRepository;

        public DeleteMessageCommandHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _emailMessageRepository = new Mock<IEmailMessageRepository>();
            _unitOfWork.Setup(x => x.EmailMessages).Returns(_emailMessageRepository.Object);
            _handler = new DeleteMessageCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task ReturnFailureWhenInvalid()
        {
            // Arrange
            var request = new DeleteMessageCommand("invalid_id");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Invalid message ID format");
        }

        [Fact]
        public async Task ReturnSuccesWhenDeleted()
        {
            // Arrange
            var objectId = ObjectId.GenerateNewId();
            var message = new EmailMessage
            {
                Id = objectId,
                Subject = "Test",
                Body = "Test"
            };

            _emailMessageRepository
                .Setup(x => x.GetByIdAsync(objectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(message);

            _emailMessageRepository
                .Setup(x => x.DeleteAsync(objectId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var request = new DeleteMessageCommand(objectId.ToString());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Deleted");

            _emailMessageRepository.Verify(x => x.DeleteAsync(objectId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
