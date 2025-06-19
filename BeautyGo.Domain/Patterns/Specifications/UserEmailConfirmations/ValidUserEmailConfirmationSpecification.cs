using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailConfirmations;

public class ValidUserEmailConfirmationSpecification : Specification<UserEmailConfirmation>
{
    private readonly DateTime _currentDate;

    public ValidUserEmailConfirmationSpecification(DateTime currentDate) => _currentDate = currentDate;

    public override Expression<Func<UserEmailConfirmation, bool>> ToExpression() =>
        p => !p.IsUsed &&
              p.ExpiresAt > _currentDate; 
}
