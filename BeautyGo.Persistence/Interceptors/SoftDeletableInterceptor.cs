using BeautyGo.Domain.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautyGo.Persistence.Interceptors;

public class SoftDeletableInterceptor : SaveChangesInterceptor
{
    #region Utilities

    private void UpdateSoftDeletableEntities(DateTime currentDate, DbContext context)
    {
        var deletedEntries = context.ChangeTracker
            .Entries<ISoftDeletableEntity>()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            MarkAsSoftDeleted(entry, currentDate);
            SetEntityStateToModified(entry);
            ResetDeletedReferences(entry);
        }
    }

    private void MarkAsSoftDeleted(EntityEntry<ISoftDeletableEntity> entry, DateTime currentDate)
    {
        entry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = currentDate;
    }

    private void SetEntityStateToModified(EntityEntry entry)
    {
        entry.State = EntityState.Modified;
    }

    private void ResetDeletedReferences(EntityEntry entry)
    {
        if (!entry.References.Any())
            return;

        foreach (var reference in entry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
        {
            reference.TargetEntry.State = EntityState.Unchanged;
            ResetDeletedReferences(reference.TargetEntry);
        }
    }

    #endregion

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
            UpdateSoftDeletableEntities(DateTime.Now, eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
