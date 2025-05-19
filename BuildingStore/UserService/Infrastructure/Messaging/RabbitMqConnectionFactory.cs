using RabbitMQ.Client;

namespace UserService.Infrastructure.Messaging
{
    /// <summary>
    /// Конфигурация для подключения к RabbitMQ.
    /// </summary>
    public class RabbitMqConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public RabbitMqConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IConnection> CreateConnectionAsync(CancellationToken cancellation = default)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };

            return await factory.CreateConnectionAsync(cancellation);
        }
    }
}
