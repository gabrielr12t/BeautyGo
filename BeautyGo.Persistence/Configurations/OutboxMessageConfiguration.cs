using BeautyGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations;

internal class OutboxMessageConfiguration : BaseEntityConfiguration<OutboxMessage>
{
    public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessage");

        base.Configure(builder);
    }
}
