using BeautyGo.Domain.Entities.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Stores;

internal class StorePictureConfiguration : BaseEntityConfiguration<StorePicture>
{
    public override void Configure(EntityTypeBuilder<StorePicture> builder)
    {
        base.Configure(builder);

        builder.ToTable("StorePicturies", "Store");

        builder.HasOne(sp => sp.Store)
            .WithMany(s => s.Pictures)
            .HasForeignKey(sp => sp.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Picture)
            .WithMany()
            .HasForeignKey(sp => sp.PictureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sp => new { sp.StoreId, sp.PictureId })
            .IsUnique();
    }
}
