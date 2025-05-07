using Microsoft.AspNetCore.Identity;
using System.Threading;
using UserService.Application.Interfaces;
using UserService.Domain.DataBase;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Services
{
    /// <summary>
    /// Аутенфикация и регистрация пользователя.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthenticationService(IPasswordHasherService passwordHasher, IUnitOfWork unitOfWork,
         IJwtGenerator jwtGenerator)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _jwtGenerator = jwtGenerator;   
        }

        /// <inheritdoc/>
        public async Task<AppUser> RegisterAsync(string name, string email, string password, CancellationToken cancellation)
        {
            var hashedPassword = _passwordHasher.GeneratePasswordHash(password);

            var user = new AppUser()
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
