using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public class AndSpecification<T> : Specification<T>
    where T : BaseEntity
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;

        CombineIncludes(left, right);
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpression, parameter),
            Expression.Invoke(rightExpression, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
