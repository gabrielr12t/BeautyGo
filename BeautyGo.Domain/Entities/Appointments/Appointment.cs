using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.Entities.Appointments;

public class Appointment : BaseEntity
{
    public DateTime DateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public decimal TotalAmountAtBooking { get; set; } // Valor total do agendamento no momento da reserva (fixo), para o cliente não agendar um preço e o profissional aumentar

    public Guid ProfessionalId { get; set; }
    public Professional Professional { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Feedback Feedback { get; set; }

    public ICollection<WaitingList> WaitingLists { get; set; }
    public ICollection<AppointmentService> Services { get; set; }
}
