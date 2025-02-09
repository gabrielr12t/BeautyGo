using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;

public class ValidUserEmailValidationTokenSpecification : Specification<UserEmailTokenValidation>
{
    private readonly DateTime _currentDate;

    public ValidUserEmailValidationTokenSpecification(DateTime currentDate) => _currentDate = currentDate;

    public override Expression<Func<UserEmailTokenValidation, bool>> ToExpression() =>
        p => !p.IsUsed &&
              p.ExpiresAt > _currentDate;
}
