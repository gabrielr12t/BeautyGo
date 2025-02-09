using BeautyGo.Domain.Entities.Business;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Business;

public class BusinessByOwnerIdSpecification : Specification<BeautyBusiness>
{
    private readonly Guid _ownerId;

    public BusinessByOwnerIdSpecification(Guid ownerId) => _ownerId = ownerId;

    public override Expression<Func<BeautyBusiness, bool>> ToExpression() =>
        business => business.CreatedId == _ownerId;
}
