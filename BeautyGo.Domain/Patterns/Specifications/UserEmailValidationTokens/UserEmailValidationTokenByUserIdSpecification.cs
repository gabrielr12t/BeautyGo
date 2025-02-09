using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;

public class UserEmailValidationTokenByUserIdSpecification : Specification<UserEmailTokenValidation>
{
    private readonly Guid _userId;

    public UserEmailValidationTokenByUserIdSpecification(Guid userId) => 
        _userId = userId;

    public override Expression<Func<UserEmailTokenValidation, bool>> ToExpression() =>
        p => p.UserId == _userId;
}
