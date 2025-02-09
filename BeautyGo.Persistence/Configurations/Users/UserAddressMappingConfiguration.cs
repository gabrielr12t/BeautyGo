using BeautyGo.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Users;

internal class UserAddressMappingConfiguration : BaseEntityConfiguration<UserAddressMapping>
{
    public override void Configure(EntityTypeBuilder<UserAddressMapping> builder)
    {
        base.Configure(builder);

        builder.ToTable("UsersAddresses", "User");

        builder.HasKey(uam => new { uam.UserId, uam.AddressId });

        builder.HasIndex(p => new { p.UserId, p.AddressId })
            .IsUnique();

        builder.HasOne(uam => uam.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(uam => uam.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uam => uam.Address)
            .WithMany()
            .HasForeignKey(uam => uam.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
