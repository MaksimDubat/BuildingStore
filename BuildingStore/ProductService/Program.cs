using Microsoft.EntityFrameworkCore;
using PdfGenerator.Grpc;
using ProductService.Application.Services;
using ProductService.Domain.DataBase;
using ProductService.Infrastructure.Middleware;
using ProductService.WebAPI.Registrations;

namespace ProductService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddAuthorizationPolicies();
            builder.Services.AddRepositories();
            builder.Services.AddValidation();
            builder.Services.AddMediatrExtension();
            builder.Services.AddAutoMapperExtension();
            builder.Services.AddProductHostedServises();
            builder.Services.AddServices();
            builder.Services.AddMessageBroker(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddGrpc();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MutableDbContext>();
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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
