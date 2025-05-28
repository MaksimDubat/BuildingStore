using FluentAssertions;
using MongoDB.Bson;
using Moq;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Domain.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NotificationServiceUnitTests
{
    public class UpdateMessageCommandHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly UpdateMessageCommandHandler _handler;
        private readonly Mock<IEmailMessageRepository> _emailMessageRepository;

        public UpdateMessageCommandHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _emailMessageRepository = new Mock<IEmailMessageRepository>();
            _unitOfWork.Setup(x => x.EmailMessages).Returns(_emailMessageRepository.Object);
            _handler = new UpdateMessageCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task ReturnsSuccessWhenUpdated()
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
                .Setup(x => x.UpdateAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()));

            var updatedModel = new MessageModel
            {
                Subject = "New Subject",
                Body = "New Body"
            };

            var command = new UpdateMessageCommand(objectId.ToString(), updatedModel);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Updated");
            message.Subject.Should().Be("New Subject");
            message.Body.Should().Be("New Body");
        }

        [Fact]
        public async Task ReturnFailureWhenInvalidId()
        {
            // Arrange
            var command = new UpdateMessageCommand("Test_id", new MessageModel());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Invalid message ID format");
        }

        [Fact]
        public async Task ReturnFailureWhenNotFound()
        {
            // Arrange
            var objectId = ObjectId.GenerateNewId();

            _emailMessageRepository
                .Setup(x => x.GetByIdAsync(objectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EmailMessage)null);

            var command = new UpdateMessageCommand(objectId.ToString(), new MessageModel());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Not found");
        }
    }
}
