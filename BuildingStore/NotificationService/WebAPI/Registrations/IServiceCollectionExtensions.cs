using Hangfire;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Handlers;
using NotificationService.Application.Services;
using NotificationService.Domain.DataBase;
using NotificationService.Domain.Smtp;
using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.UnitOfWork;
using NotificationService.Infrastructure.HangfireJobs;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.Validators.MessageValidator;
using NotificationService.Application.Validators.Behavior;
using System.Text;
using NotificationService.Infrastructure.JwtSet;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


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
            services.AddScoped<IEmailsToSentRepository, EmailsToSentRepository>();
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

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AddMessageCommand>, AddMessageValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMQ"));
            services.AddSingleton<RabbitMqConnectionFactory>();
            services.AddHostedService<UserNotificationConsumer>();

            return services;
        }

        public static IServiceCollection AddHangFire(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings").Get<MongoSettings>();

            services.AddHangfire(config =>
            {
                config.UseMongoStorage(mongoSettings.ConnectionString, mongoSettings.DatabaseName, new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    Prefix = "hangfire",
                    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.Poll
                });
            });

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddJobHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<HangfireJobInitializer>();

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
    }
}
