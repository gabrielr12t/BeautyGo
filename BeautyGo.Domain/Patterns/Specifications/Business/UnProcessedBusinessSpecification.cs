using BeautyGo.Domain.Entities.Business;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Business;

public class UnProcessedBusinessSpecification : Specification<BeautyBusiness>
{
    public override Expression<Func<BeautyBusiness, bool>> ToExpression() =>
        business => !business.Processed;
}
