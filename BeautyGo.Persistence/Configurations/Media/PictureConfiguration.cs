using BeautyGo.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Media;

internal class PictureConfiguration : BaseEntityConfiguration<Picture>
{
    public override void Configure(EntityTypeBuilder<Picture> builder)
    {
        base.Configure(builder);

        builder.ToTable("Pictures", "Media");

        builder.Property(p => p.MimeType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.SeoFilename)
               .HasMaxLength(200);

        builder.Property(p => p.AltAttribute)
               .HasMaxLength(500);

        builder.Property(p => p.TitleAttribute)
               .HasMaxLength(500);

        builder.Property(p => p.VirtualPath)
               .HasMaxLength(1000);

        builder.Property(p => p.IsNew)
               .IsRequired();
    }
}
