using BeautyGo.Domain.Entities.Professionals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.AvailableHours;

public class ProfessionalAvailabilityConfiguration : BaseEntityConfiguration<ProfessionalAvailability>
{
    public override void Configure(EntityTypeBuilder<ProfessionalAvailability> builder)
    {
        builder.ToTable("ProfessionalAvailabilities", "AvailableHours");

        builder.Property(pa => pa.DayOfWeek)
            .IsRequired();

        builder.Property(pa => pa.StartTime)
            .IsRequired();

        builder.Property(pa => pa.EndTime)
            .IsRequired();

        builder.Property(pa => pa.StartLunchTime)
            .IsRequired();

        builder.Property(pa => pa.EndLunchTime)
            .IsRequired();

        builder.HasOne(pa => pa.Professional)
            .WithMany(p => p.ProfessionalAvailabilities)
            .HasForeignKey(pa => pa.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
