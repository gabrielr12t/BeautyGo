using BeautyGo.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Common;

internal class AddressConfiguration : BaseEntityConfiguration<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);

        builder.ToTable("Addresses", "Common");

        builder.Property(p => p.Street).IsRequired().HasMaxLength(100);
        builder.Property(p => p.PostalCode).IsRequired().HasMaxLength(8);
        builder.Property(p => p.City).IsRequired().HasMaxLength(50);
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(30);
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);

        builder
            .HasIndex(p => p.Latitude)
            .HasDatabaseName("IX_LATITUDE");

        builder
            .HasIndex(p => p.Longitude)
            .HasDatabaseName("IX_LONGITUDE");
    }
}
