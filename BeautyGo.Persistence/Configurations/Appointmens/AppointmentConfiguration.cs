using BeautyGo.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Appointmens;

internal class AppointmentConfiguration : BaseEntityConfiguration<Appointment>
{
    public override void Configure(EntityTypeBuilder<Appointment> builder)
    {
        base.Configure(builder);

        builder.ToTable("Appointment", "Appointments");

        builder.Property(a => a.DateTime)
           .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(a => a.TotalAmountAtBooking)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(a => a.Professional)
            .WithMany()
            .HasForeignKey(a => a.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(a => a.Customer)
            .WithMany()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(a => a.Feedback)
            .WithOne(f => f.Appointment)
            .HasForeignKey<Feedback>(f => f.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.WaitingLists)
            .WithOne()
            .HasForeignKey(w => w.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Services)
            .WithOne(asv => asv.Appointment)
            .HasForeignKey(asv => asv.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
