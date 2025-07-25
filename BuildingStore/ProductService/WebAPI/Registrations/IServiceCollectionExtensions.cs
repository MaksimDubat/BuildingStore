﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.Services;
using ProductService.Application.Validators.Behavior;
using ProductService.Application.Validators.CategoryValidation;
using ProductService.Application.Validators.ProductValidation;
using ProductService.Domain.DataBase;
using ProductService.Infrastructure.Elasticsearch;
using ProductService.Infrastructure.JwtSet;
using ProductService.Infrastructure.Messaging;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.UnitOfWork;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using SixLabors.ImageSharp;
using System.Text;

namespace ProductService.WebAPI.Registrations
{
    /// <summary>
    /// Класс регистрации компонентов.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MutableDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

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

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AddCategoryCommand>, CategoryAddValidator>();
            services.AddTransient<IValidator<UpdateCategoryCommand>, CategoryUpdateValidator>();
            services.AddTransient<IValidator<AddProductCommand>, ProductAddValidator>();
            services.AddTransient<IValidator<UpdateProductCommand>, ProductUpdateValidator>();
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

        public static IServiceCollection AddProductHostedServises(this IServiceCollection services)
        {
            services.AddHostedService<SaleCleanupService>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<PdfGenerationService>();
            services.AddScoped<ImageService>();

            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMQ"));
            services.AddSingleton<RabbitMqConnectionFactory>();
            services.AddHostedService<UserConsumer>();

            return services;
        }

        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection("ElasticOptions").Get<ElasticOptions>();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(options.Uri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "notification-service-logs-{0:yyyy.MM.dd}"
                })
        .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            return services;
        }
    }
}
