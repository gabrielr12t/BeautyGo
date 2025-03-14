using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Persons;

public class Professional : User
{
    public Professional(string firstName, string lastName, string email, string phoneNumber, string cpf)
        : base(firstName, lastName, email, phoneNumber, cpf)
    {
        Services = new List<ProfessionalService>();
        Appointments = new List<Appointment>();
        ProfessionalAvailabilities = new List<ProfessionalAvailability>();
    }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; }

    public ICollection<ProfessionalService> Services { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }

    public override async Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor)
    {
        await visitor.AssignRoleAsync(this);
    }

    public static Professional Create(string firstName, string lastName, string email, string phoneNumber, string cpf)
    {
        var professional = new Professional(firstName, lastName, email, phoneNumber, cpf);

        professional.AddDomainEvent(new UserCreatedDomainEvent(professional));

        return professional;
    }
}
