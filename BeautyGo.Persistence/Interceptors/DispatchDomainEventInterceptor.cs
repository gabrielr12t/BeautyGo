using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautyGo.Persistence.Interceptors;

public class DispatchDomainEventInterceptor(IMediator _mediator)
    : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var savedChanges = await base.SavedChangesAsync(eventData, result, cancellationToken);

        if (savedChanges > 0)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
            await DispatchEntityChangeEventsAsync(eventData.Context, cancellationToken);
        }

        return savedChanges;
    }

    private async Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(p => p.Entity.DomainEvents.Any())
            .Select(p => p.Entity);

        var domainEvents = entities
            .SelectMany(p => p.DomainEvents)
            .ToList();

        entities.ToList().ForEach(p => p.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
    }

    private async Task DispatchEntityChangeEventsAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        var entries = context.ChangeTracker
            .Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                await _mediator.Publish(new EntityInsertedEvent<BaseEntity>(entry.Entity), cancellationToken);
            }
            else if (entry.State == EntityState.Modified)
            {
                await _mediator.Publish(new EntityUpdatedEvent<BaseEntity>(entry.Entity), cancellationToken);
            }
            else if (entry.State == EntityState.Deleted)
            {
                await _mediator.Publish(new EntityDeletedEvent<BaseEntity>(entry.Entity), cancellationToken);
            }
        }
    }
}

