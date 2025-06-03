using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;

public class UserEmailValidationTokenWithUserSpecification : Specification<UserEmailConfirmation>
{
    public UserEmailValidationTokenWithUserSpecification()
    {
        AddInclude(p => p.User);
    }

    public override Expression<Func<UserEmailConfirmation, bool>> ToExpression() =>
        p => true;
}
