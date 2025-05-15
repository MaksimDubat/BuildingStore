using MongoDB.Bson.IO;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using System.Text.Json;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace NotificationService.Infrastructure.HttpClients
{
    /// <summary>
    /// Клиент для обращения в микросервис пользователей.
    /// </summary>
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public UserServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["UserService:ApiKey"];
        }

        public async Task<List<string>> GetAllUserEmailsAsync(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7130/api/v1/users?page=1&size=1000000");

            request.Headers.Add("X-Api-Key", _apiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine(content);

            UserResultDto<UserDto> users;

            try
            {
                users = JsonSerializer.Deserialize<UserResultDto<UserDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize user data.", ex);
            }

            if (users == null || users.Data.Any(u => string.IsNullOrWhiteSpace(u.UserEmail)))
            {
                throw new Exception("Some users have invalid data.");
            }

            return users.Data.Select(x => x.UserEmail).ToList(); 
        }
    }
}
