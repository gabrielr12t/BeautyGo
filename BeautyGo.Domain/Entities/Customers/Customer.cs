using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Entities.Customers;

public class Customer : User
{
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }
}
