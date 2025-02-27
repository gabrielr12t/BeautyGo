using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Repositories;

public interface IOutboxMessageRepository : IBaseRepository<OutboxMessage>
{
    Task<ICollection<OutboxMessage>> GetRecentUnprocessedOutboxMessages(int size);
}
