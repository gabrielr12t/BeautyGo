using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.Entities.Appointments;

public class WaitingList : BaseEntity
{
    public WaitingListStatus Status { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? NotifiedAt { get; set; }  // Data e hora que o cliente foi notificado

    public DateTime? AcceptedAt { get; set; }  // Data e hora que o cliente aceitou o horário

    public DateTime? RejectedAt { get; set; }  // Data e hora que o cliente rejeitou o horário

    public DateTime? TimeoutAt { get; set; }  // Data e hora que o tempo limite expirou (se aplicável)

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
}
