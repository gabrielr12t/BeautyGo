using BeautyGo.Domain.Entities.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Outbox;

internal class OutboxMessageConfiguration : BaseEntityConfiguration<OutboxMessage>
{
    public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessage");

        builder
            .HasMany(o => o.Errors)
            .WithOne(e => e.OutboxMessage)
            .HasForeignKey(e => e.OutboxMessageId)
            .IsRequired();

        base.Configure(builder);
    }
}
