using BeautyGo.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Media;

internal class PictureBinaryConfiguration : BaseEntityConfiguration<PictureBinary>
{
    public override void Configure(EntityTypeBuilder<PictureBinary> builder)
    {
        base.Configure(builder);

        builder.ToTable("PictureBinaries", "Media");

        builder.Property(pb => pb.PictureId)
            .IsRequired();

        builder.Property(pb => pb.BinaryData)
           .IsRequired()
           .HasColumnType("varbinary(max)");

        builder.HasOne(pb => pb.Picture)
            .WithMany(p => p.PictureBinaries)
            .HasForeignKey(pb => pb.PictureId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
