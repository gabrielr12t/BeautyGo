using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.UserEmailConfirmations;

public class UserEmailConfirmationByUserIdSpecification : Specification<UserEmailConfirmation>
{
    private readonly Guid _userId;

    public UserEmailConfirmationByUserIdSpecification(Guid userId) => 
        _userId = userId;

    public override Expression<Func<UserEmailConfirmation, bool>> ToExpression() =>
        p => p.UserId == _userId;
}
