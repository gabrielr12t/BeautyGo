using BeautyGo.Domain.Entities.Outbox;

namespace BeautyGo.Domain.Repositories;

public interface IOutboxMessageRepository : IBaseRepository<OutboxMessage>
{
    Task<ICollection<OutboxMessage>> GetRecentUnprocessedOutboxMessages(int size, CancellationToken cancellation = default);

    Task UpdateAsync(OutboxMessage message);
}
