using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public class NotSpecification<T> : Specification<T>
    where T : BaseEntity
{
    private readonly Specification<T> _specification;

    public NotSpecification(Specification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = _specification.ToExpression();
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.Not(Expression.Invoke(expression, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
