using Microsoft.Extensions.Options;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Collections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService.Infrastructure.Messaging
{
    /// <summary>
    /// Потребитель данных из сервиса пользователей.
    /// </summary>
    public class UserNotificationConsumer : BackgroundService
    {
        private readonly RabbitMqConnectionFactory _connectionFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConfig _configOptions;

        private IConnection _connection;
        private IChannel _channel;
        private readonly string _exchangeName;
        private readonly string _queueName;

        public UserNotificationConsumer(RabbitMqConnectionFactory connectionFactory, IServiceProvider serviceProvider, IOptions<RabbitMqConfig> configOptions)
        {
            _connectionFactory = connectionFactory;
            _serviceProvider = serviceProvider;

            _configOptions = configOptions.Value;
            _exchangeName = _configOptions.ExchangeName;
            _queueName = _configOptions.QueueName;
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

        protected async override Task ExecuteAsync(CancellationToken cancellation)
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


                var emailsToSend = new EmailsToSent
                {
                    Email = user.UserEmail
                };

                var exist = await unitOfWork.EmailsToSent.AnyAsync(p => p.Email == emailsToSend.Email, cancellation);

                if (!exist)
                {
                    await unitOfWork.EmailsToSent.AddEntityAsync(emailsToSend, cancellation);
                }

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}
