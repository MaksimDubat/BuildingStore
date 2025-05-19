
using Hangfire;
using NotificationService.Application.Services;

namespace NotificationService.Infrastructure.HangfireJobs
{
    /// <summary>
    /// Создание задачи.
    /// </summary>
    public class HangfireJobInitializer : IHostedService
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public HangfireJobInitializer(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager; 
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
             _recurringJobManager.AddOrUpdate<EmailSender>(
                "daily-newsletter-job",
                sender => sender.SendNewsletterAsync(cancellationToken),
                Cron.Minutely);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
