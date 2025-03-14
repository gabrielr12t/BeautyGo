using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Persons;
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

    public BusinessOwner PromoteToOwner()
    {
        var owner = new BusinessOwner(FirstName, LastName, Email, PhoneNumber, Cpf)
        {
            Id = Id,
            EmailConfirmed = EmailConfirmed,
            EmailToRevalidate = EmailToRevalidate,
            CannotLoginUntilDate = CannotLoginUntilDate,
            IsActive = IsActive,
            LastActivityDate = LastActivityDate,
            LastLoginDate = LastLoginDate,
            MustChangePassword = MustChangePassword,
            Passwords = Passwords,
            Addresses = Addresses,
            UserRoles = UserRoles,
            ValidationTokens = ValidationTokens,
            CreatedOn = CreatedOn,
            DateOfBirth = DateOfBirth
        };

        owner.ChangeIpAddress(LastIpAddress);

        return owner;
    }
}
