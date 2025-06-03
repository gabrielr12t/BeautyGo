using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailConfirmations;

public class UserEmailConfirmationByTokenSpecification : Specification<UserEmailConfirmation>
{
    private readonly string _token;

    public UserEmailConfirmationByTokenSpecification(string token) => _token = token;

    public override Expression<Func<UserEmailConfirmation, bool>> ToExpression() =>
        p => p.Token == _token;
}
