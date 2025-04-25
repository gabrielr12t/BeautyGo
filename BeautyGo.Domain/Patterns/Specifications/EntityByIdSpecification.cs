using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public class EntityByIdSpecification<TEntity> : Specification<TEntity>
    where TEntity : BaseEntity
{
    private readonly Guid[] _ids;

    public EntityByIdSpecification(params Guid[] id) =>
        _ids = id;

    public EntityByIdSpecification(IReadOnlyList<string> includes = null, params Guid[] id)
    {
        _ids = id;
        AddInclude(includes);
    }

    public override Expression<Func<TEntity, bool>> ToExpression() =>
        entity => _ids.Contains(entity.Id);
}
