using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Collections;
using NotificationService.Domain.DataBase;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Реализация паттерна UnitOfWork.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private readonly NotificationDbContext _context;
        private IClientSessionHandle? _session;

        public IEmailMessageRepository EmailMessages { get; }
        public IEmailsToSentRepository EmailsToSent {  get; }

        public UnitOfWork(IMongoClient mongoClient, IConfiguration configuration)
        {
            var databaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;
            _database = mongoClient.GetDatabase(databaseName);
            _context = new NotificationDbContext(_database);

            EmailMessages = new EmailMessageRepository(_context);
            EmailsToSent = new EmailsToSentRepository(_context);
        }

        /// <inheritdoc/>
        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            return new BaseRepository<T>(_context);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _session?.Dispose();
        }

    }
}
