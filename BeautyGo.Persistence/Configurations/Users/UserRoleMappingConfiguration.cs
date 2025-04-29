using BeautyGo.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Users;

internal class UserRoleMappingConfiguration : BaseEntityConfiguration<UserRoleMapping>
{
    public override void Configure(EntityTypeBuilder<UserRoleMapping> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserRolesMapping", "Users");

        builder.HasKey(p => new { p.UserId, p.UserRoleId });

        builder
            .Property(ur => ur.Id)
            .HasDefaultValueSql("NEWID()");  

        builder.HasIndex(p => new { p.UserId, p.UserRoleId })
            .IsUnique();

        builder.HasOne(p => p.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.UserRole)
            .WithMany(p => p.UserRoleMappings)
            .HasForeignKey(p => p.UserRoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
