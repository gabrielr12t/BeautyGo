using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Patterns.Specifications.OutboxMessages;
using BeautyGo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BeautyGo.Persistence.Repositories;

internal class OutboxMessagerepository : BaseRepository<OutboxMessage>, IOutboxMessageRepository
{
    public OutboxMessagerepository(BeautyGoContext context) : base(context)
    {
    }

    public async Task<ICollection<OutboxMessage>> GetRecentUnprocessedOutboxMessages(int size)
    {
        var unProcessedOutboxMessagesSpec = new UnprocessedOutboxMessagesSpecification(5);

        var query = Query(true);

        query = query.Where(unProcessedOutboxMessagesSpec.ToExpression());

        query = query.OrderByDescending(p => p.CreatedOn);

        return await query.ToListAsync();
    }
}
