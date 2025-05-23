using NotificationService.WebAPI.Middleware;
using NotificationService.WebAPI.Registrations;

namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMongoDatabase(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddSmtpConfiguration(builder.Configuration);
            builder.Services.AddMediatrExtension();
            builder.Services.AddValidation();
            builder.Services.AddMessageBroker(builder.Configuration);
            builder.Services.AddHangFire(builder.Configuration);
            builder.Services.AddJobHostedServices();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddAuthorizationPolicies();
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
