using BeautyGo.Domain.Entities.Security;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.RefreshTokens;

public class RefreshTokenByUserIdSpecification : Specification<RefreshToken>
{
    private readonly Guid _userId;

    public RefreshTokenByUserIdSpecification(Guid userId)
    {
        _userId = userId;
    }

    public override Expression<Func<RefreshToken, bool>> ToExpression() =>
        r => r.UserId == _userId;
}
