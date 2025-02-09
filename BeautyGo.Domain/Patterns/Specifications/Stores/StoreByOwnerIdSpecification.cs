using BeautyGo.Domain.Entities.Stores;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Stores;

public class StoreByOwnerIdSpecification : Specification<Store>
{
    private readonly Guid _ownerId;

    public StoreByOwnerIdSpecification(Guid ownerId) => _ownerId = ownerId;

    public override Expression<Func<Store, bool>> ToExpression() =>
        store => store.OwnerId == _ownerId;
}
