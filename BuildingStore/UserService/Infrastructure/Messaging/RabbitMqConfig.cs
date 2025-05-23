namespace UserService.Infrastructure.Messaging
{
    /// <summary>
    /// Класс конфигурации RabbitMQ
    /// </summary>
    public class RabbitMqConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
    }
}
