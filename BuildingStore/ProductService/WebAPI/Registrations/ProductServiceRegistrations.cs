using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.Services;
using ProductService.Application.Validators.Behavior;
using ProductService.Application.Validators.CategoryValidation;
using ProductService.Application.Validators.ProductValidation;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.UnitOfWork;

namespace ProductService.WebAPI.Registrations
{
    /// <summary>
    /// Класс регистрации компонентов.
    /// </summary>
    public class ProductServiceRegistrations
    {
        public static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MutableDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IValidator<AddCategoryCommand>, CategoryAddValidator>();
            services.AddTransient<IValidator<UpdateCategoryCommand>, CategoryUpdateValidator>();
            services.AddTransient<IValidator<AddProductCommand>, ProductAddValidator>();
            services.AddTransient<IValidator<UpdateProductCommand>, ProductUpdateValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddHostedService<SaleCleanupService>();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ProductServiceRegistrations).Assembly));
        }                                                                                                       
    }
}
