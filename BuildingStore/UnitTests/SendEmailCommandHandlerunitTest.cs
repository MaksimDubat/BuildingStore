using FluentAssertions;
using MongoDB.Driver;
using Moq;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Application.Services;
using NotificationService.Domain.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NotificationServiceUnitTests
{
    public class SendEmailCommandHandlerunitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IEmailMessageRepository> _emailMessageRepository;
        private readonly Mock<IEmailsToSentRepository> _emailsToSentRepository;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly SendEmailCommandHandler _handler;

        public SendEmailCommandHandlerunitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _emailMessageRepository = new Mock<IEmailMessageRepository>();
            _emailsToSentRepository = new Mock<IEmailsToSentRepository>();
            _emailSender = new Mock<IEmailSender>();
            _handler = new SendEmailCommandHandler(_emailSender.Object, _unitOfWork.Object);
            _unitOfWork.Setup(x => x.EmailMessages).Returns(_emailMessageRepository.Object);
            _unitOfWork.Setup(x => x.EmailsToSent).Returns(_emailsToSentRepository.Object);
        }

        [Fact]
        public async Task ReturnSuccessWhenSent()
        {
            // Arrange
            var subject = "Test";
            var body = "Test";
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
            };

            var users = new List<EmailsToSent>
            {
                new EmailsToSent { Email = "test1@example.com" },
                new EmailsToSent { Email = "test2@example.com" }
            };

            _emailMessageRepository.Setup(x => x.GetMessageBySubjectAsync(subject, It.IsAny<CancellationToken>()))
                .ReturnsAsync(message);

            _emailsToSentRepository.Setup(x => x.GetAllAsync(It.IsAny<Func<IFindFluent<EmailsToSent, EmailsToSent>, IFindFluent<EmailsToSent, EmailsToSent>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _emailSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var command = new SendEmailCommand(subject);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Emails were send");
            _emailSender.Verify(x => x.SendEmailAsync("test1@example.com", subject, "Test"), Times.Once);
            _emailSender.Verify(x => x.SendEmailAsync("test2@example.com", subject, "Test"), Times.Once);
        }
    }
}
