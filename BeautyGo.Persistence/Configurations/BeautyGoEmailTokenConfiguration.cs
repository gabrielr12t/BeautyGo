using BeautyGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations;

internal class BeautyGoEmailTokenConfiguration : BaseEntityConfiguration<EmailTokenValidation>
{
    public override void Configure(EntityTypeBuilder<EmailTokenValidation> builder)
    {
        base.Configure(builder);

        builder.UseTpcMappingStrategy();
    }
}
