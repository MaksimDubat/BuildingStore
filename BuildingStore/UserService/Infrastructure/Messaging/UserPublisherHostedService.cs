namespace UserService.Infrastructure.Messaging
{
    /// <summary>
    /// Фоновый сервис для публикатора.
    /// </summary>
    public class UserPublisherHostedService : BackgroundService
    {
        private readonly UserNotificationPublisher _publisher;

        public UserPublisherHostedService(UserNotificationPublisher publisher)
        {
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            await _publisher.InitializeAsync(cancellation);
        }
    }

}
