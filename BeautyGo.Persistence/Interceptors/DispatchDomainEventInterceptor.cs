using BeautyGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautyGo.Persistence.Interceptors;

public class DispatchDomainEventInterceptor(IMediator _mediator)
    : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var savedChanges = base.SavedChanges(eventData, result);

        if (savedChanges > 0)
            DispatchDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();

        return savedChanges;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var savedChanges = await base.SavedChangesAsync(eventData, result, cancellationToken);

        if (savedChanges > 0)
            await DispatchDomainEventsAsync(eventData.Context);

        return savedChanges;
    }

    private async Task DispatchDomainEventsAsync(DbContext context)
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
            await _mediator.Publish(domainEvent);
    }
}

