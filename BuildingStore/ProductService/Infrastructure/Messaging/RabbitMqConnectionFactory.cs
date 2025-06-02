using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ProductService.Infrastructure.Messaging
{
    /// <summary>
    /// Конфигурация для подключения к RabbitMQ.
    /// </summary>
    public class RabbitMqConnectionFactory
    {
        private readonly RabbitMqConfig _config;

        public RabbitMqConnectionFactory(IOptions<RabbitMqConfig> config)
        {
            _config = config.Value;
        }

        public async Task<IConnection> CreateConnectionAsync(CancellationToken cancellation = default)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.Host,
                Port = _config.Port,
                UserName = _config.Username,
                Password = _config.Password,
            };

            return await factory.CreateConnectionAsync(cancellation);
        }
    }
}
