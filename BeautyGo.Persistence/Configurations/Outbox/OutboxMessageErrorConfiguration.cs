using BeautyGo.Domain.Entities.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Outbox;

public class OutboxMessageErrorConfiguration : BaseEntityConfiguration<OutboxMessageError>
{
    public override void Configure(EntityTypeBuilder<OutboxMessageError> builder)
    {
        builder.ToTable("OutboxMessageError");

        base.Configure(builder);
    }
}
