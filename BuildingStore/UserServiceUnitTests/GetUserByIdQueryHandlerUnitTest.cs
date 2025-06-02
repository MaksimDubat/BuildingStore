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
using UserService.Application.MediatrConfiguration.Handlers;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Domain.Entities;
using Xunit;

namespace UserServiceUnitTests
{
    public class GetUserByIdQueryHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserProfileCacheService> _profileCacheService;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();
            _profileCacheService = new Mock<IUserProfileCacheService>();
            _unitOfWork.Setup(u => u.Users).Returns(_userRepository.Object);
            _handler = new GetUserByIdQueryHandler(_unitOfWork.Object, _mapper.Object, _profileCacheService.Object);
        }

        [Fact]
        public async Task ReturnUserFromCache()
        {
            // Arrange
            var userId = 1;
            var cachedUser = new UserDto { Id = userId, UserEmail = "Test@test.com" };

            _profileCacheService
                .Setup(x => x.SetProfileAsync(userId, cachedUser, TimeSpan.FromMinutes(2), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _profileCacheService
                .Setup(x => x.GetProfileAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cachedUser);

            var request = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(cachedUser);
        }

        [Fact]
        public async Task returnFailureWhenNotFound()
        {
            // Arrange
            var userId = 1;

            _profileCacheService
                .Setup(x => x.GetProfileAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto)null!);

            _userRepository
                .Setup(x => x.GetAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((AppUser)null!);

            var request = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("user not found");
        }

        [Fact]
        public async Task ReturnMappedUserFromDb()
        {
            // Act
            var userId = 1;
            var user = new AppUser { Id = userId, UserEmail = "Test@test.com" };
            var mappedUser = new UserDto { Id = userId, UserEmail = "Test@test.com" };

            _profileCacheService
                .Setup(x => x.GetProfileAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserDto)null!);

            _userRepository
                .Setup(x => x.GetAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapper
                .Setup(x => x.Map<UserDto>(user))
                .Returns(mappedUser);

            var request = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(mappedUser);
        }
    }
}
