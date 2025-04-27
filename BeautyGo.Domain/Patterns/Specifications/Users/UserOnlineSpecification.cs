using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public sealed class UserOnlineSpecification : Specification<User>
{
    private readonly DateTime _lastActivityFrom = DateTime.Now.AddMinutes(-10);

    public UserOnlineSpecification(Expression<Func<User, object>> orderByDesc = null)
    {
        ApplyOrderByDesc(orderByDesc ??= user => user.LastActivityDate);
    }

    public override Expression<Func<User, bool>> ToExpression() =>
        user => user.LastActivityDate >= _lastActivityFrom;
}
