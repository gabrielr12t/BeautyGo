using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.EmailTokenValidations;

public class EmailValidationTokenValidSpecification : Specification<EmailTokenValidation>
{
    private readonly DateTime _currentDate;

    public EmailValidationTokenValidSpecification(DateTime currentDate)
    {
        _currentDate = currentDate;
    }

    public override Expression<Func<EmailTokenValidation, bool>> ToExpression() =>
       p => !p.IsUsed &&
             p.ExpiresAt > _currentDate;
}
