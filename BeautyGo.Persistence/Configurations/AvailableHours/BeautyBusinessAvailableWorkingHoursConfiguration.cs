using BeautyGo.Domain.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.AvailableHours;

public class BeautyBusinessAvailableWorkingHoursConfiguration : BaseEntityConfiguration<BusinessWorkingHours>
{
    public override void Configure(EntityTypeBuilder<BusinessWorkingHours> builder)
    {
        builder.ToTable("BusinessWorkingHours", "AvailableHours");

        builder.Property(bwh => bwh.Day)
            .IsRequired();

        builder.Property(bwh => bwh.OpenTime)
            .IsRequired();

        builder.Property(bwh => bwh.CloseTime)
            .IsRequired();

        builder.HasOne(bwh => bwh.BeautyBusiness)
            .WithMany(bb => bb.BusinessWorkingHours)
            .HasForeignKey(bwh => bwh.BeautyBusinessId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
