using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Collections;
using NotificationService.Domain.DataBase;

namespace NotificationService.Infrastructure.Repositories
{
    public class EmailsToSentRepository : BaseRepository<EmailsToSent>, IEmailsToSentRepository
    {
        private readonly IMongoCollection<EmailsToSent> _collection;

        public EmailsToSentRepository(NotificationDbContext context) : base(context)
        {
            _collection = context.GetCollection<EmailsToSent>();
        }

        /// <inheritdoc/>
        public async Task<EmailsToSent> GetByEmailAsync(string email, CancellationToken cancellation)
        {
            var filter = Builders<EmailsToSent>.Filter.Eq(x => x.Email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellation);
        }
    }
}
