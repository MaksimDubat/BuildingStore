using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Application.Services;
using NotificationService.Domain.DataBase;
using NotificationService.Domain.Smtp;
using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.RedisCache;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.UnitOfWork;


namespace NotificationService.WebAPI.Registrations
{
    /// <summary>
    /// Класс регистрации компонентов.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddScoped(sp =>
            {
                var mongoSettings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(mongoSettings.DatabaseName);
                return new NotificationDbContext(database);
            });

            return services;

        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEmailMessageRepository, EmailMessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddSmtpConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddTransient<EmailSender>();

            return services;
        }

        public static IServiceCollection AddMediatrExtension(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(IServiceCollectionExtensions).Assembly));

            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMqConnectionFactory>();
            services.AddHostedService<UserNotificationConsumer>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserEmailCacheService, UserEmailCacheService>();  

            return services;
        }

        public static IServiceCollection AddEmailsCache(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6380";
                options.InstanceName = "UserService_";
            });

            return services;
        }
    }
}
