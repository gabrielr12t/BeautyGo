using BeautyGo.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Events;

internal class EventErrorConfiguration : BaseEntityConfiguration<EventError>
{
    public override void Configure(EntityTypeBuilder<EventError> builder)
    {
        base.Configure(builder);

        builder.ToTable("EventError", "Events");
    }
}
