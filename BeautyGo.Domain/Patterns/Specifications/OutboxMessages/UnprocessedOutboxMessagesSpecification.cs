using BeautyGo.Domain.Entities.Outbox;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.OutboxMessages;

public sealed class UnprocessedOutboxMessagesSpecification : Specification<OutboxMessage>
{
    public UnprocessedOutboxMessagesSpecification(int quantity)
    {
        Size(quantity);
    }

    public override Expression<Func<OutboxMessage, bool>> ToExpression() =>
        outboxMessage => outboxMessage.ProcessedOn == null && !outboxMessage.ProcessedOn.HasValue;
}
