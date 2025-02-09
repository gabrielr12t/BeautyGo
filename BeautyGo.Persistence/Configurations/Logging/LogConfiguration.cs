using BeautyGo.Domain.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Logging;

internal class LogConfiguration : BaseEntityConfiguration<Log>
{
    public override void Configure(EntityTypeBuilder<Log> builder)
    {
        base.Configure(builder);

        builder.ToTable("Logs", "Logging");
    }
}
