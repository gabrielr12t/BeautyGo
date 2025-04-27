using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.OutboxMessages;

public sealed class UnprocessedOutboxMessagesSpecification : Specification<OutboxMessage>
{
    public UnprocessedOutboxMessagesSpecification(int quantity)
    {
        Size(quantity);
    }

    public override Expression<Func<OutboxMessage, bool>> ToExpression() =>
        outboxMessage => outboxMessage.ProcessedOn == null && outboxMessage.Attempts <= 3;
}
