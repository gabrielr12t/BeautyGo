using BeautyGo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace BeautyGo.Persistence.Interceptors;

public sealed class OutboxEventDispatcherInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var outboxMessages = dbContext.ChangeTracker
            .Entries<BaseEntity>()
            .Select(p => p.Entity)
            .SelectMany(p =>
            {
                var domainEvents = p.DomainEvents;

                p.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.Now,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
