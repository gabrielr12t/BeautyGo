using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public class EntityByIdsSpecification<TEntity> : Specification<TEntity>
    where TEntity : BaseEntity
{
    private readonly IReadOnlyList<Guid> _ids;

    public EntityByIdsSpecification(IReadOnlyList<Guid> ids)
    {
        _ids = ids;
    }

    public override Expression<Func<TEntity, bool>> ToExpression() =>
        entity => _ids.Contains(entity.Id);
}
