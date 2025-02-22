using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Professionals;

namespace BeautyGo.Domain.Patterns.Visitor.Users;

public interface IUserRoleHandlerVisitor
{
    Task AssignRoleAsync(Customer customer, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(Professional professional, CancellationToken cancellationToken = default);
}

