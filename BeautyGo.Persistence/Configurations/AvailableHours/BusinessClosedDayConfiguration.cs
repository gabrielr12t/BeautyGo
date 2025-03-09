using BeautyGo.Domain.Entities.Businesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.AvailableHours;

internal class BusinessClosedDayConfiguration : BaseEntityConfiguration<BusinessClosedDay>
{
    public override void Configure(EntityTypeBuilder<BusinessClosedDay> builder)
    {
        builder.ToTable("BusinessClosedDays", "AvailableHours");

        builder.Property(bwh => bwh.ClosedDate)
            .IsRequired();

        builder.HasOne(bwh => bwh.Business)
            .WithMany(bb => bb.ClosedDays)
            .HasForeignKey(bwh => bwh.BusinessId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(); 
        
        base.Configure(builder);
    }
}
