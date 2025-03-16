using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.Patterns.Visitor.Users;

public interface IUserRoleHandlerVisitor
{
    Task AssignRoleAsync(Customer customer, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(Professional professional, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(BusinessOwner owner, CancellationToken cancellationToken = default);
}

