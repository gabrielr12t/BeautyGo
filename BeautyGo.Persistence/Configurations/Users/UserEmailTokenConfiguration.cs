using BeautyGo.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Users;

internal class UserEmailTokenConfiguration : BaseEntityConfiguration<UserEmailConfirmation>
{
    public override void Configure(EntityTypeBuilder<UserEmailConfirmation> builder)
    {
        builder.ToTable("EmailTokens", "Users");

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.Token)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.ExpiresAt)
            .IsRequired();

        builder.Property(e => e.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(e => e.Token)
            .IsUnique();

        builder.HasIndex(e => e.UserId);

        builder.Navigation(p => p.User)
            .AutoInclude();

        builder.HasOne(p => p.User)
            .WithMany(p => p.ValidationTokens)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
