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
    public class GetAllUsersQueryHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly GetAllUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerUnitTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();
            _unitOfWork.Setup(u => u.Users).Returns(_userRepository.Object);
            _handler = new GetAllUsersQueryHandler(_unitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public async Task ReturnsMappedUsers()
        {
            // Arrange
            var users = new List<AppUser>
            {
                new AppUser { Id = 1, UserEmail = "test@gmail.com"},
                new AppUser { Id = 2, UserEmail = "test2@gmail.com"}
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Id = 1, UserEmail = "test@gmail.com" },
                new UserDto { Id = 2, UserEmail = "test2@gmail.com" }
            };

            _userRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Func<IQueryable<AppUser>, IQueryable<AppUser>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapper
                .Setup(m => m.Map<IEnumerable<UserDto>>(users))
                .Returns(userDtos);

            var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(userDtos);
            result.Message.Should().Be("Users");
        }

        [Fact]
        public async Task ReturnEmptyList()
        {
            var users = new List<AppUser>();
            var userDtos = new List<UserDto>();

            _userRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Func<IQueryable<AppUser>, IQueryable<AppUser>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            _mapper
                .Setup(m => m.Map<IEnumerable<UserDto>>(users))
                .Returns(userDtos);

            var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();
            result.Message.Should().Be("Users");
        }
    }
}
