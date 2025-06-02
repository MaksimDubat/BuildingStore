using Microsoft.Extensions.Options;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Infrastructure.Messaging
{
    /// <summary>
    /// Потребитель данных из сервиса пользователей.
    /// </summary>
    public class UserConsumer : BackgroundService
    {
        private readonly RabbitMqConnectionFactory _connectionFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConfig _config;

        private IConnection _connection;
        private IChannel _channel;
        private readonly string _exchangeName;
        private readonly string _queueName;

        public UserConsumer(RabbitMqConnectionFactory connectionFactory, IServiceProvider serviceProvider, IOptions<RabbitMqConfig> config)
        {
            _connectionFactory = connectionFactory;
            _serviceProvider = serviceProvider;

            _config = config.Value;
            _exchangeName = _config.ExchangeName;
            _queueName = _config.QueueName;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: _exchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                     { "x-queue-mode", "lazy" }
                },
                cancellationToken: cancellationToken);

            await _channel.QueueBindAsync(queue: _queueName, exchange: _exchangeName, routingKey: "", cancellationToken: cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();

                var user = JsonSerializer.Deserialize<UserResultDto>(Encoding.UTF8.GetString(body), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                using var scope = _serviceProvider.CreateScope();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var cart = new Cart
                {
                    CartId = user.Id,
                    UserId = user.Id,
                };

                var exist = await unitOfWork.Carts.AnyAsync(p => p.UserId == cart.UserId, cancellation);

                if (!exist)
                {
                    await unitOfWork.Carts.AddAsync(cart, cancellation);
                    await unitOfWork.CompleteAsync(cancellation);
                }

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}
