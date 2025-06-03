using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.EmailTokenValidations;

public class EmailTokenValidationByTokenSpecification : Specification<EmailConfirmation>
{
    private readonly string _token;

    public EmailTokenValidationByTokenSpecification(string token)
    {
        _token = token;
    }

    public override Expression<Func<EmailConfirmation, bool>> ToExpression() =>
        p => string.Equals(p.Token, _token);
}
