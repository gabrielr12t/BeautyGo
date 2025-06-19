using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserRoles;

public sealed class UserRoleByUserIdSpecification : Specification<UserRoleMapping>
{
    private readonly Guid _userId;

    public UserRoleByUserIdSpecification(Guid userId)
    {
        _userId = userId; 
    }

    public override Expression<Func<UserRoleMapping, bool>> ToExpression() =>
        userRole => userRole.UserId == _userId;
}
