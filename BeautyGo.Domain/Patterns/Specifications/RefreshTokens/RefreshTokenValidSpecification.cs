using BeautyGo.Domain.Entities.Security;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.RefreshTokens;

public class RefreshTokenValidSpecification : Specification<RefreshToken>
{
    public override Expression<Func<RefreshToken, bool>> ToExpression() =>
        r => r.IsValid();
}
