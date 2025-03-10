using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Customers;

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

    public Professional PromoteToProfessional()
    {
        var professional = new Professional(FirstName, LastName, Email, PhoneNumber, Cpf);
        
        professional.EmailConfirmed = EmailConfirmed;
        professional.EmailToRevalidate = EmailToRevalidate;
        professional.CannotLoginUntilDate = CannotLoginUntilDate;
        professional.IsActive = IsActive;
        professional.Id = Id;
        professional.LastActivityDate = LastActivityDate;
        professional.ChangeIpAddress(LastIpAddress);
        professional.LastLoginDate = LastLoginDate;
        professional.MustChangePassword = MustChangePassword;
        professional.Passwords = Passwords;
        professional.Addresses = Addresses;
        professional.UserRoles = UserRoles;
        professional.Appointments = Appointments;
        professional.ValidationTokens = ValidationTokens;

        return professional;
    }
}
