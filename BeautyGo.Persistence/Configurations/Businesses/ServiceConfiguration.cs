using BeautyGo.Domain.Entities.Businesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Businesses;

public class ServiceConfiguration : BaseEntityConfiguration<Service>
{
    public override void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services", "Businesses");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.Duration)
            .IsRequired();

        builder.HasOne(s => s.BeautyBusiness)
            .WithMany(bb => bb.Services)
            .HasForeignKey(s => s.BeautyBusinessId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(s => s.Pictures)
            .WithOne()
            .HasForeignKey(sp => sp.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Profissionals)
            .WithOne(ps => ps.Service)
            .HasForeignKey(ps => ps.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
