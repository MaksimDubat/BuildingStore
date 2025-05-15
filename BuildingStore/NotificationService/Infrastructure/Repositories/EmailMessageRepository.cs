using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Collections;
using NotificationService.Domain.DataBase;

namespace NotificationService.Infrastructure.Repositories
{
    /// <summary>
    /// Реопзиторий по работе с email сообщениями.
    /// </summary>
    public class EmailMessageRepository : BaseRepository<EmailMessage>, IEmailMessageRepository
    {
        private readonly IMongoCollection<EmailMessage> _collection;

        public EmailMessageRepository(NotificationDbContext context) : base(context)
        {
            _collection = context.GetCollection<EmailMessage>();
        }

        /// <inheritdoc/>
        public async Task<EmailMessage> GetMessageBySubjectAsync(string subject, CancellationToken cancellation)
        {
            var filter = Builders<EmailMessage>.Filter.Eq(x => x.Subject, subject);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellation);
        }
    }
}
