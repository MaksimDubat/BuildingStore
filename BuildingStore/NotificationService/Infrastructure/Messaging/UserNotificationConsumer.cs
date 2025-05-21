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

        private IConnection _connection;
        private IChannel _channel;
        private const string ExchangeName = "user_notifications_exchange";
        private const string QueueName = "product_user_notifications_queue";

        public UserNotificationConsumer(RabbitMqConnectionFactory connectionFactory, IServiceProvider serviceProvider)
        {
            _connectionFactory = connectionFactory;
            _serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: ExchangeName,
               type: ExchangeType.Fanout,
               durable: true,
               autoDelete: false,
               arguments: null,
               cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                     { "x-queue-mode", "lazy" }
                },
                cancellationToken: cancellationToken);

            await _channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: "", cancellationToken: cancellationToken);

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

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);
        }
    }
}
