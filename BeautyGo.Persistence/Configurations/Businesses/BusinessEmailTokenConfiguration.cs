using BeautyGo.Domain.Entities.Businesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Businesses;

internal class BusinessEmailTokenConfiguration : BaseEntityConfiguration<BusinessEmailTokenValidation>
{
    public override void Configure(EntityTypeBuilder<BusinessEmailTokenValidation> builder)
    {
        builder.ToTable("EmailTokens", "Businesses");

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
