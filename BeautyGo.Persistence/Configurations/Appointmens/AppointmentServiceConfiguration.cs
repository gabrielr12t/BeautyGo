using BeautyGo.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyGo.Persistence.Configurations.Appointmens;

internal class AppointmentServiceConfiguration : BaseEntityConfiguration<AppointmentService>
{
    public override void Configure(EntityTypeBuilder<AppointmentService> builder)
    {
        base.Configure(builder);

        builder.ToTable("AppointmentService", "Appointments");

        builder.HasOne(asv => asv.Appointment)
            .WithMany(a => a.Services)
            .HasForeignKey(asv => asv.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(asv => asv.Service)
            .WithMany()
            .HasForeignKey(asv => asv.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
