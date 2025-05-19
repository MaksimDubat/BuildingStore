using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Messaging
{
    /// <summary>
    /// Публикатор данных для сервиса уведомлений.
    /// </summary>
    public class UserNotificationPublisher
    {
        private IConnection _connection;
        private IChannel _channel;
        private Timer _timer;

        private readonly RabbitMqConnectionFactory _connectionFactory;
        private readonly IServiceScopeFactory _scopeFactory;

        private int _lastUserId = 0;
        private const int PageSize = 50;

        public UserNotificationPublisher(RabbitMqConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory)
        {
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
        }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            _connection = await _connectionFactory.CreateConnectionAsync(cancellation);

            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: "user_notifications_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                     { "x-queue-mode", "lazy" }
                },
                cancellationToken: cancellation);

            _timer = new Timer(async _ => await PublishUsersBatchAsync(cancellation), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private async Task PublishUsersBatchAsync(CancellationToken cancellation)
        {
            using var scope = _scopeFactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var users = await unitOfWork.Users.GetAllAsync(
                q => q
                    .Where(u => u.Id > _lastUserId)
                    .OrderBy(u => u.Id)
                    .Take(PageSize),
                cancellation);

            foreach (var user in users)
            {
                var message = JsonSerializer.Serialize(user);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = new BasicProperties
                {
                    Persistent = true,
                    ContentType = "application/json",
                };

                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "user_notifications_queue",
                    mandatory: true,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellation);
            }

            var lastUser = users.LastOrDefault();

            if (lastUser is not null)
            {
                _lastUserId = lastUser.Id;
            }
        }

    }
}
