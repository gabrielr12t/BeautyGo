using BeautyGo.Domain.DomainEvents;
using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Persons;

public class Customer : User
{
    public Customer(string firstName, string lastName, string email, string phoneNumber, string cpf)
        : base(firstName, lastName, email, phoneNumber, cpf)
    {
        Appointments = new List<Appointment>();
        Feedbacks = new List<Feedback>();
    }

    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }

    public override async Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor)
    {
        await visitor.AssignRoleAsync(this);
    }

    public static Customer Create(string firstName, string lastName, string email, string phoneNumber, string cpf)
    {
        var customer = new Customer(firstName, lastName, email, phoneNumber, cpf);

        customer.AddDomainEvent(new UserCreatedDomainEvent(customer));

        return customer;
    } 
}
