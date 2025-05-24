using Microsoft.EntityFrameworkCore;
using System.Configuration;
using UserService.Domain.DataBase;
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
            builder.Services.AddLoggingConfiguration(builder.Configuration);
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
            builder.Services.AddMessageBroker(builder.Configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MutableDbConext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
