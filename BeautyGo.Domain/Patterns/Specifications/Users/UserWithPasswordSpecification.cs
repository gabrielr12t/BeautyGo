using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public class UserWithPasswordSpecification : Specification<User>
{
    public UserWithPasswordSpecification()
    {
        AddInclude(p => p.Passwords);
    }

    public override Expression<Func<User, bool>> ToExpression() =>
        p => true;
}
