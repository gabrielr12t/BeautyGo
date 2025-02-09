using BeautyGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations;

internal class AuditEntryConfiguration : BaseEntityConfiguration<AuditEntry>
{
    public override void Configure(EntityTypeBuilder<AuditEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable("AuditEntries");

        builder.HasOne(ae => ae.ModifiedBy)
            .WithMany()
            .HasForeignKey(ae => ae.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(ae => ae.EntityName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ae => ae.EntityId)
            .IsRequired();

        builder.Property(ae => ae.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ae => ae.ActionTimestamp)
            .IsRequired();

        builder.Property(ae => ae.Old)
            .HasMaxLength(1000);

        builder.Property(ae => ae.Current)
            .HasMaxLength(1000);

        builder.Property(ae => ae.ChangedProperties)
            .HasMaxLength(1000);

        builder.HasIndex(ae => new { ae.EntityName, ae.EntityId })
            .HasDatabaseName("IX_EntityName_EntityId");

        builder.HasIndex(ae => ae.ActionTimestamp)
            .HasDatabaseName("IX_ActionTimestamp");
    }
}
