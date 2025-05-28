using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Application.MediatConfiguration.Queries;
using NotificationService.Domain.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NotificationServiceUnitTests
{
    public class GetAllMessagesQueryHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IEmailMessageRepository> _emailMessageRepository;
        private readonly GetAllMessagesQueryHandler _handler;

        public GetAllMessagesQueryHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _emailMessageRepository = new Mock<IEmailMessageRepository>();
            _unitOfWork.Setup(x => x.EmailMessages).Returns(_emailMessageRepository.Object);
            _handler = new GetAllMessagesQueryHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task ReturnSuccessWithMessages()
        {
            var messages = new List<EmailMessage>
            {
                new EmailMessage { Id = ObjectId.GenerateNewId(), Subject = "Subject 1", Body = "Body 1" },
                new EmailMessage { Id = ObjectId.GenerateNewId(), Subject = "Subject 2", Body = "Body 2" }
            };

            _emailMessageRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Func<IFindFluent<EmailMessage, EmailMessage>, IFindFluent<EmailMessage, EmailMessage>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(messages);
            
            int number = 1;
            int size = 10;

            var query = new GetAllMessagesQuery(number, size);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            result.Data.First().Subject.Should().Be("Subject 1");
            result.Message.Should().Be("Messages");

        }

        [Fact]
        public async Task ReturnSuccessWithEmpty()
        {
            // Arrange
            _emailMessageRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Func<IFindFluent<EmailMessage, EmailMessage>, IFindFluent<EmailMessage, EmailMessage>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EmailMessage>());

            int number = 1;
            int size = 10;

            var query = new GetAllMessagesQuery(number, size);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();
            result.Message.Should().Be("Messages");
        }

        [Fact]
        public async Task ReturnCorrectMappedDto()
        {
            // Arrange
            var objectId = ObjectId.GenerateNewId();
            var email = new EmailMessage { Id = objectId, Subject = "Test Subj", Body = "Test Body" };

            _emailMessageRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Func<IFindFluent<EmailMessage, EmailMessage>, IFindFluent<EmailMessage, EmailMessage>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EmailMessage> { email });

            int number = 1;
            int size = 10;

            var query = new GetAllMessagesQuery(number, size);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Should().ContainSingle();
            var dto = result.Data.First();
            dto.Id.Should().Be(objectId.ToString());
            dto.Subject.Should().Be("Test Subj");
            dto.Body.Should().Be("Test Body");
        }
    }
}
