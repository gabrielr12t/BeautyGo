using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public class UserIsInRoleSpecification : Specification<User>
{
    private readonly string _roleName;

    public UserIsInRoleSpecification(string roleName) =>
        _roleName = roleName?.ToLower();

    public override Expression<Func<User, bool>> ToExpression()
    {
        var userRolesSpec = new UserWithRolesSpecification();

        And(userRolesSpec);

        return user => user.UserRoles.Any(p => p.UserRole.Description.ToLower() == _roleName);
    }
}
