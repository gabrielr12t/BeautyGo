using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Business;

public class BusinessByOwnerIdSpecification : Specification<Entities.Businesses.Business>
{
    private readonly Guid _ownerId;

    public BusinessByOwnerIdSpecification(Guid ownerId) => _ownerId = ownerId;

    public override Expression<Func<Entities.Businesses.Business, bool>> ToExpression() =>
        business => business.CreatedId == _ownerId;
}
