using BeautyGo.Domain.Entities.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Stores;

internal class StoreEmailTokenConfiguration : BaseEntityConfiguration<StoreEmailTokenValidation>
{
    public override void Configure(EntityTypeBuilder<StoreEmailTokenValidation> builder)
    {
        builder.ToTable("EmailTokens", "Store");

        builder.Property(e => e.StoreId)
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

        builder.HasIndex(e => e.StoreId);

        builder.HasOne(p => p.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
