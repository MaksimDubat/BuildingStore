using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Services;
using UserService.Application.Validators.Behavior;
using UserService.Application.Validators.UserValidation;
using UserService.Domain.DataBase;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.JwtSet;
using UserService.Infrastructure.RedisCache;
using UserService.Infrastructure.RefreshTokenSet;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.WebAPI.Registrations
{
    /// <summary>
    /// Класс регистрации компонентов.
    /// </summary>
    public class UserServiceRegistrations
    {
        public static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MutableDbConext>(options =>
                options.UseNpgsql(connectionString)
            );

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "UserService_";
            });

            services.AddHttpContextAccessor();

            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = JwtBearerEventsHandlers.OnMessageReceived
                    };

                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("AdminManagerPolicy", policy =>
                    policy.RequireRole("Admin", "Manager"));

                options.AddPolicy("ManagerPolicy", policy =>
                    policy.RequireRole("Manager"));

                options.AddPolicy("GuestPolicy", policy =>
                    policy.RequireRole("Guest"));

                options.AddPolicy("UserOrGuestPolicy", policy =>
                    policy.RequireRole("User", "Guest"));

                options.AddPolicy("ManagerAdminUserPolicy", policy =>
                    policy.RequireRole("User", "Admin", "Manager"));
            });

            services.AddScoped<IUserProfileCacheService, UserProfileCacheService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IValidator<RegisterCommand>, RegistrationValidator>();
            services.AddTransient<IValidator<RegisterManagersCommand>, RegistrationManagerValidator>();
            services.AddTransient<IValidator<LoginCommand>, LoginValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UserServiceRegistrations).Assembly));
        }

    }
}
