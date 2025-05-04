using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace BeautyGo.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public AuditableEntitySaveChangesInterceptor(IUserIdentifierProvider userIdentifierProvider)
    {
        _userIdentifierProvider = userIdentifierProvider;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var auditEntries = GetAuditEntries(DateTime.Now, context);
        if (auditEntries.Any())
            context.Set<AuditEntry>().AddRange(auditEntries);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private List<AuditEntry> GetAuditEntries(DateTime timestamp, DbContext context)
    {
        var auditEntries = new List<AuditEntry>();

        foreach (var entityEntry in context.ChangeTracker.Entries().Where(e => e.Entity is IAuditableEntity))
        {
            var auditEntry = CreateAuditEntry(entityEntry, timestamp);
            if (auditEntry != null)
                auditEntries.Add(auditEntry);
        }

        return auditEntries;
    }

    private AuditEntry CreateAuditEntry(EntityEntry entityEntry, DateTime timestamp)
    {
        var auditEntry = new AuditEntry
        {
            EntityName = entityEntry.Entity.GetType().Name,
            EntityId = (Guid)entityEntry.Property("Id").CurrentValue,
            ActionTimestamp = timestamp,
            UserId = _userIdentifierProvider?.UserId,
            Action = entityEntry.State.ToString()
        };

        switch (entityEntry.State)
        {
            case EntityState.Modified:
                auditEntry.Old = SerializeValues(entityEntry.OriginalValues);
                auditEntry.Current = SerializeValues(entityEntry.CurrentValues);
                auditEntry.ChangedProperties = GetChangedProperties(entityEntry);
                break;
            case EntityState.Added:
                auditEntry.Current = SerializeValues(entityEntry.CurrentValues);
                break;
            case EntityState.Deleted:
                auditEntry.Old = SerializeValues(entityEntry.OriginalValues);
                break;
        }

        return auditEntry;
    }

    private string SerializeValues(PropertyValues values)
    {
        var dictionary = values.Properties.ToDictionary(
            property => property.Name,
            property => values[property]?.ToString() ?? string.Empty
        );

        return JsonConvert.SerializeObject(dictionary);
    }

    private string GetChangedProperties(EntityEntry entityEntry)
    {
        return JsonConvert.SerializeObject(entityEntry.Properties
            .Where(p => p.IsModified)
            .Select(p => p.Metadata.Name));
    }
}
