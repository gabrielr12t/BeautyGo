using BeautyGo.Domain.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Business;

internal class BeautyBusinessEmailTokenConfiguration : BaseEntityConfiguration<BeautyBusinessEmailTokenValidation>
{
    public override void Configure(EntityTypeBuilder<BeautyBusinessEmailTokenValidation> builder)
    {
        builder.ToTable("EmailTokens", "Business");

        builder.Property(e => e.BusinessId)
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

        builder.HasIndex(e => e.BusinessId);

        builder.HasOne(p => p.Business)
            .WithMany(p => p.ValidationTokens)
            .HasForeignKey(p => p.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
