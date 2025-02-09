using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public class OrSpecification<T> : Specification<T>
    where T : BaseEntity
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
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
        var body = Expression.OrElse(
            Expression.Invoke(leftExpression, parameter),
            Expression.Invoke(rightExpression, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
