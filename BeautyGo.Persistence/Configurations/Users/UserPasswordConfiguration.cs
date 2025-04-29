using BeautyGo.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Users;

internal class UserPasswordConfiguration : BaseEntityConfiguration<UserPassword>
{
    public override void Configure(EntityTypeBuilder<UserPassword> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserPasswords", "Users");

        builder.HasOne(p => p.User)
            .WithMany(u => u.Passwords)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Propriedades
        builder.Property(up => up.Password)
            .IsRequired()
            .HasMaxLength(255);   

        builder.Property(up => up.Salt)
            .IsRequired()
            .HasMaxLength(255);  

        builder.HasIndex(up => up.UserId)
            .IsUnique();  
    }
}
