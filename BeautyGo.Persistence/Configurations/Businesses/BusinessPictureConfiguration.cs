using BeautyGo.Domain.Entities.Businesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Businesses;

internal class BusinessPictureConfiguration : BaseEntityConfiguration<BusinessPicture>
{
    public override void Configure(EntityTypeBuilder<BusinessPicture> builder)
    {
        base.Configure(builder);

        builder.ToTable("BeautyBusinessPicturies", "Businesses");

        builder.HasOne(sp => sp.BeautyBusiness)
            .WithMany(s => s.Pictures)
            .HasForeignKey(sp => sp.BeautyBusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Picture)
            .WithMany()
            .HasForeignKey(sp => sp.PictureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sp => new { sp.BeautyBusinessId, sp.PictureId })
            .IsUnique();
    }
}
