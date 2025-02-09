using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;

public class UserEmailValidationTokenByTokenSpecification : Specification<UserEmailTokenValidation>
{
    private readonly string _token;

    public UserEmailValidationTokenByTokenSpecification(string token) => _token = token;

    public override Expression<Func<UserEmailTokenValidation, bool>> ToExpression() =>
        p => p.Token == _token;
}
