using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautyGo.Persistence.Interceptors;

public class DispatchDomainEventInterceptor(IDomainEventDispatcher _eventDispatcher)
    : SaveChangesInterceptor
{
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
            await _eventDispatcher.PublishAsync(domainEvent, cancellationToken);
    }

    #endregion

    #region Overrides

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var savedChanges = await base.SavedChangesAsync(eventData, result, cancellationToken);

        if (savedChanges > 0)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return savedChanges;
    } 

    #endregion 
}

