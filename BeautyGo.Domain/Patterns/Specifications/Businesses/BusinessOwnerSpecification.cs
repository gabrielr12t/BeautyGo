using BeautyGo.Domain.Entities.Businesses;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Businesses;

public class BusinessOwnerSpecification : Specification<Business>
{
    private readonly Guid _userId;

    public BusinessOwnerSpecification(Guid userId)
    {
        _userId = userId;
    }

    public override Expression<Func<Business, bool>> ToExpression() =>
        business => business.OwnerId == _userId;
}
