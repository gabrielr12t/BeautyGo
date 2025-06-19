using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautyGo.Persistence.Interceptors;

public class DispatchDomainEventInterceptor(IMediator _mediator)
    : SaveChangesInterceptor
{
    private record EntitySnapshot(EntityState State, BaseEntity Entity, Type EntityType);
    private readonly List<EntitySnapshot> _entryEntitiesChanged = new();

    #region Utilities

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
        foreach (var entry in _entryEntitiesChanged)
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

        _entryEntitiesChanged.Clear();
    }

    #endregion

    #region Overrides

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .Select(p => new EntitySnapshot(p.State, p.Entity, p.GetType()))
            .ToList();

        _entryEntitiesChanged.AddRange(entries);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

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

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        _entryEntitiesChanged?.Clear();

        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    #endregion 
}

