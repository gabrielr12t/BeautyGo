using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailConfirmations;

public class UserEmailConfirmationWithUserSpecification : Specification<UserEmailConfirmation>
{
    public UserEmailConfirmationWithUserSpecification()
    {
        AddInclude(p => p.User);
    }

    public override Expression<Func<UserEmailConfirmation, bool>> ToExpression() =>
        p => true;
}
