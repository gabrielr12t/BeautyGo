using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public class UserWithRolesSpecification : Specification<User>
{
    public UserWithRolesSpecification()
    {
        AddInclude(p => p.UserRoles);
        AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRoleMapping.UserRole)}");
    }

    public override Expression<Func<User, bool>> ToExpression() =>
        user => true;
}
