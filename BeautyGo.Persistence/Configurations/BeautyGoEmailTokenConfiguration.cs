using BeautyGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations;

internal class BeautyGoEmailTokenConfiguration : BaseEntityConfiguration<BeautyGoEmailTokenValidation>
{
    public override void Configure(EntityTypeBuilder<BeautyGoEmailTokenValidation> builder)
    {
        base.Configure(builder);

        builder.UseTpcMappingStrategy();
    }
}
