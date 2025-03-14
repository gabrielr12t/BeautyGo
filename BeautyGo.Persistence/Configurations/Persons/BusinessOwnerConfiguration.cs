using BeautyGo.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Persons;

internal class BusinessOwnerConfiguration : BaseEntityConfiguration<BusinessOwner>
{
    public override void Configure(EntityTypeBuilder<BusinessOwner> builder)
    {
        builder.HasMany(p => p.Businesses)
            .WithOne(ps => ps.Owner)
            .HasForeignKey(ps => ps.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
