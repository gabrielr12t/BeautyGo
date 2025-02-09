using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserRoles;

public class UserRoleByDescriptionSpecification : Specification<UserRole>
{
    private readonly string _role;

    public UserRoleByDescriptionSpecification(string role)
    {
        _role = role;
    }

    public override Expression<Func<UserRole, bool>> ToExpression() =>
        userRole => userRole.Description == _role;
}
