using BeautyGo.Domain.Entities.Outbox;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Domain.Repositories;

public interface IOutboxMessageRepository : IEFBaseRepository<OutboxMessage>
{
    Task<ICollection<OutboxMessage>> GetRecentUnprocessedOutboxMessages(int size, CancellationToken cancellation = default);

    //Task UpdateAsync(OutboxMessage message);
}
