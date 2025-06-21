using BeautyGo.Domain.Core.Events;
using MediatR;
using System.Reflection;

namespace BeautyGo.Infrastructure.Core.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync<TDomainEvent>(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
        where TDomainEvent : IDomainEvent
    {
        await _mediator.Publish(domainEvent, cancellationToken);

        var notificationType = domainEvent.GetType();
        var baseEventType = notificationType.GetTypeInfo().BaseType;

        if (baseEventType is null)
            return;

        var isEntityChangeEvent = baseEventType.IsGenericType &&
            (
                baseEventType.GetGenericTypeDefinition() == typeof(EntityInsertedDomainEvent<>) ||
                baseEventType.GetGenericTypeDefinition() == typeof(EntityUpdatedDomainEvent<>) ||
                baseEventType.GetGenericTypeDefinition() == typeof(EntityDeletedDomainEvent<>)
            );

        if (!isEntityChangeEvent)
            return;

        var entity = baseEventType.GetProperty("Entity")?.GetValue(domainEvent);

        if (entity is null)
            return;

        var genericEventType = baseEventType.GetGenericTypeDefinition()
            .MakeGenericType(entity.GetType());

        if (notificationType == genericEventType)
            return; 

        var genericEvent = Activator.CreateInstance(genericEventType, entity);

        if (genericEvent is IDomainEvent genericNotification)
        {
            await _mediator.Publish(genericNotification, cancellationToken);
        }
    }
}