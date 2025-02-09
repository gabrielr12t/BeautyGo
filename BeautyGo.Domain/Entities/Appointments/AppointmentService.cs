using BeautyGo.Domain.Entities.Business;

namespace BeautyGo.Domain.Entities.Appointments;

public class AppointmentService : BaseEntity
{
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
}
