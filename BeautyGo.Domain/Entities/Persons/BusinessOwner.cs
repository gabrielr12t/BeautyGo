using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Persons;

public class BusinessOwner : User, IUserPromotable
{
    public BusinessOwner()
    {
        Businesses = new List<Business>();
    }

    public BusinessOwner(string firstName, string lastName, string email, string phoneNumber, string cpf)
        : base(firstName, lastName, email, phoneNumber, cpf)
    {
        Businesses = new List<Business>();
    }

    public ICollection<Business> Businesses { get; set; }

    public override async Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor)
    {
        await visitor.AssignRoleAsync(this);
    }

    public void PromoteSpecificProperties(Guid? businessId = null)
    {
        ChangeIpAddress(LastIpAddress);
    }
}
