using BeautyGo.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Appointmens;

public class WaitingListConfiguration : BaseEntityConfiguration<WaitingList>
{
    public override void Configure(EntityTypeBuilder<WaitingList> builder)
    {
        builder.ToTable("WaitingLists", "Appointments");

        builder.Property(w => w.Status)
            .IsRequired();

        builder.Property(w => w.RequestedDate)
            .IsRequired();

        builder.Property(w => w.NotifiedAt)
            .IsRequired(false);

        builder.Property(w => w.AcceptedAt)
            .IsRequired(false);

        builder.Property(w => w.RejectedAt)
            .IsRequired(false);

        builder.Property(w => w.TimeoutAt)
            .IsRequired(false);

        builder.HasOne(w => w.Customer)
            .WithMany()
            .HasForeignKey(w => w.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(w => w.Appointment)
            .WithMany()
            .HasForeignKey(w => w.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
