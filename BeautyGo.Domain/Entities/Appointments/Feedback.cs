using BeautyGo.Domain.Entities.Customers;

namespace BeautyGo.Domain.Entities.Appointments;

public class Feedback : BaseEntity
{
    public bool IsApproved { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public string Reply { get; set; }

    //NÃO POSSUI NEM O PROFISSIONAL NEM CLIENTE POIS JÁ ESTÃO NO AGENDAMENTO ?????
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
}
