using BeautyGo.Domain.Entities.Businesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.AvailableHours;

public class BusinessAvailableWorkingHoursConfiguration : BaseEntityConfiguration<BusinessWorkingHours>
{
    public override void Configure(EntityTypeBuilder<BusinessWorkingHours> builder)
    {
        builder.ToTable("BusinessWorkingHours", "AvailableHours");

        builder.Property(bwh => bwh.Day)
            .IsRequired();

        builder.Property(bwh => bwh.OpeningTime)
            .IsRequired();

        builder.Property(bwh => bwh.ClosingTime)
            .IsRequired();

        //builder.HasOne(bwh => bwh.Business)
        //    .WithMany(bb => bb.WorkingHours)
        //    .HasForeignKey(bwh => bwh.BusinessId)
        //    .OnDelete(DeleteBehavior.ClientCascade)
        //    .IsRequired();
    }
}
