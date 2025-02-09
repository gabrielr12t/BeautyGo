using BeautyGo.Domain.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Business;

internal class BeautyBusinessPictureConfiguration : BaseEntityConfiguration<BeautyBusinessPicture>
{
    public override void Configure(EntityTypeBuilder<BeautyBusinessPicture> builder)
    {
        base.Configure(builder);

        builder.ToTable("BeautyBusinessPicturies", "Business");

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
