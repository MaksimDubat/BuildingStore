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

        private const int PageSize = 50;
        private const string ExchangeName = "user_notifications_exchange";

        public UserNotificationPublisher(RabbitMqConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory)
        {
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
        }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            _connection = await _connectionFactory.CreateConnectionAsync(cancellation);

            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: ExchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellation);

            _timer = new Timer(async _ => await PublishUsersBatchAsync(cancellation), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private async Task PublishUsersBatchAsync(CancellationToken cancellation)
        {
            using var scope = _scopeFactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var users = await unitOfWork.Users.GetAllAsync(
                q => q
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
                    exchange: ExchangeName,
                    routingKey: "",
                    mandatory: true,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellation);
            }

        }

    }
}
