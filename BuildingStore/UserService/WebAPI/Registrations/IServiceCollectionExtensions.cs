using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Services;
using UserService.Application.Validators.Behavior;
using UserService.Application.Validators.UserValidation;
using UserService.Domain.DataBase;
using UserService.Infrastructure.JwtSet;
using UserService.Infrastructure.Messaging;
using UserService.Infrastructure.RedisCache;
using UserService.Infrastructure.RefreshTokenSet;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.WebAPI.Registrations
{
    /// <summary>
    /// Класс регистрации компонентов.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MutableDbConext>(options =>
                options.UseNpgsql(connectionString)
            );

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "redis";
                options.InstanceName = "UserService_";
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
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

            return services;
        }

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("AdminManagerPolicy", policy =>
                    policy.RequireRole("Admin", "Manager"));

                options.AddPolicy("ManagerPolicy", policy =>
                    policy.RequireRole("Manager"));

                options.AddPolicy("ManagerAdminUserPolicy", policy =>
                    policy.RequireRole("User", "Admin", "Manager"));
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddScoped<IUserProfileCacheService, UserProfileCacheService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterCommand>, RegistrationValidator>();
            services.AddTransient<IValidator<RegisterManagersCommand>, RegistrationManagerValidator>();
            services.AddTransient<IValidator<LoginCommand>, LoginValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddMediatrExtension(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(IServiceCollectionExtensions).Assembly));

            return services;
        }

        public static IServiceCollection AddAutoMapperExtension(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMQ"));
            services.AddSingleton<UserNotificationPublisher>();
            services.AddSingleton<RabbitMqConnectionFactory>();
            services.AddHostedService<UserPublisherHostedService>();

            return services;
        }
    }
}
