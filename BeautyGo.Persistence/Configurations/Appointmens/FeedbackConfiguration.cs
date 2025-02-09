using BeautyGo.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Appointmens;

public class FeedbackConfiguration : BaseEntityConfiguration<Feedback>
{
    public override void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks", "Appointments");

        builder.Property(f => f.IsApproved)
            .IsRequired();

        builder.Property(f => f.Rating)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(f => f.Comment)
            .HasMaxLength(1000);

        builder.Property(f => f.Reply)
            .HasMaxLength(1000);

        builder.HasOne(f => f.Appointment)
            .WithOne(a => a.Feedback)
            .HasForeignKey<Feedback>(f => f.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
