using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;

public class UserEmailValidationTokenWithUserSpecification : Specification<UserEmailTokenValidation>
{
    public UserEmailValidationTokenWithUserSpecification()
    {
        AddInclude(p => p.User);
    }

    public override Expression<Func<UserEmailTokenValidation, bool>> ToExpression() =>
        p => true;
}
