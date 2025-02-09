using BeautyGo.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Users;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserRoles", "User");

        builder.Property(p => p.Description).IsRequired().HasMaxLength(25);
    }
}
