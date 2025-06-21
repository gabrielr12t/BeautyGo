namespace BeautyGo.Domain.Core.Events;

public interface IDomainEventDispatcher
{
    Task PublishAsync<TDomainEvent>(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
        where TDomainEvent : IDomainEvent;
}
