using System.Configuration;
using UserService.WebAPI.Middleware;
using UserService.WebAPI.Registrations;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDatabase(builder.Configuration);

            builder.Services.AddRedisCache();

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddAuthorizationPolicies();

            builder.Services.AddApplicationServices();

            builder.Services.AddRepositories();

            builder.Services.AddValidation();

            builder.Services.AddMediatrExtension();

            builder.Services.AddAutoMapperExtension();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
