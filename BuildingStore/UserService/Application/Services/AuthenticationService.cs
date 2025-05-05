using Microsoft.AspNetCore.Identity;
using System.Threading;
using UserService.Domain.DataBase;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.JwtSet;

namespace UserService.Application.Services
{
    /// <summary>
    /// Аутенфикация и регистрация пользователя.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MutableDbConext _context;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthenticationService(IPasswordHasherService passwordHasher, IUnitOfWork unitOfWork,
            MutableDbConext context, IJwtGenerator jwtGenerator)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _context = context;
            _jwtGenerator = jwtGenerator;   
        }

        /// <inheritdoc/>
        public async Task<User> RegisterAsync(string name, string email, string password, CancellationToken cancellation)
        {
            var hashedPassword = _passwordHasher.GeneratePasswordHash(password);

            var user = new User()
            {
                UserName = name,
                UserEmail = email,
                PasswordHash = hashedPassword,
                Role = UserRole.User,
            };

            var exist = await _unitOfWork.Users.IsUserExistOrDuplicateAsync(user, cancellation);

            if (exist)
            {
                throw new ArgumentException("Already Exist");
            }

            return user;
        }

        /// <inheritdoc/>
        public async Task<string> SignInAsync(string email, string password, CancellationToken cancellation)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email, cancellation);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var result = _passwordHasher.VerifyPasswordHash(password, user.PasswordHash);

            if(result == false)
            {
                throw new UnauthorizedAccessException("wrong password");
            }

            return _jwtGenerator.GenerateToken(user);;
        }

    }
}
