using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;

public class UserEmailValidationTokenByTokenSpecification : Specification<UserEmailConfirmation>
{
    private readonly string _token;

    public UserEmailValidationTokenByTokenSpecification(string token) => _token = token;

    public override Expression<Func<UserEmailConfirmation, bool>> ToExpression() =>
        p => p.Token == _token;
}
