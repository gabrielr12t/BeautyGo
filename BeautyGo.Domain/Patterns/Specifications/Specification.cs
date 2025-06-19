using BeautyGo.Domain.Entities;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public abstract class Specification<T>
    where T : BaseEntity
{
    public List<Expression<Func<T, object>>> IncludeExpression { get; }
    public List<string> IncludeStrings { get; }

    public Expression<Func<T, object>> OrderByExpression { get; private set; }
    public Expression<Func<T, object>> ThenByExpression { get; private set; }

    public Expression<Func<T, object>> OrderByDescExpression { get; private set; }
    public Expression<Func<T, object>> ThenByDescExpression { get; private set; }
    public List<Func<IQueryable<T>, IQueryable<T>>> Includes { get; } = new();

    public int? Limit { get; private set; }

    protected Specification()
    {
        IncludeExpression = new();
        IncludeStrings = new();
    }

    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity) =>
        ToExpression().Compile().Invoke(entity);

    protected void CombineIncludes(Specification<T> left, Specification<T> right)
    {
        IncludeExpression.AddRange(left.IncludeExpression);
        IncludeStrings.AddRange(right.IncludeStrings);

        OrderByExpression = left.OrderByExpression ?? right.OrderByExpression;
        ThenByExpression = left.ThenByExpression ?? right.ThenByExpression;

        OrderByDescExpression = left.OrderByDescExpression ?? right.OrderByDescExpression;
        ThenByDescExpression = left.ThenByDescExpression ?? right.ThenByDescExpression;
    }

    public Specification<T> And(Specification<T> specification) => new AndSpecification<T>(this, specification);
    public Specification<T> Or(Specification<T> specification) => new OrSpecification<T>(this, specification);
    public Specification<T> Not() => new NotSpecification<T>(this);

    public Specification<T> AddInclude(Expression<Func<T, object>> includeExpression)
    {
        IncludeExpression.Add(includeExpression);
        return this;
    }

    public Specification<T> AddInclude(IReadOnlyList<string> includes)
    {
        IncludeStrings.AddRange(includes);
        return this;
    }

    public Specification<T> AddInclude(string include)
    {
        IncludeStrings.Add(include);
        return this;
    }

    public Specification<T> AddInclude(params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        Includes.AddRange(includes);
        return this;
    }

    public Specification<T> ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
        return this;
    }

    public Specification<T> ApplyThenBy(Expression<Func<T, object>> thenByExpression)
    {
        ThenByExpression = thenByExpression;
        return this;
    }

    public Specification<T> ApplyOrderByDesc(Expression<Func<T, object>> orderByExpression)
    {
        OrderByDescExpression = orderByExpression;
        return this;
    }

    public Specification<T> ApplyThenByDesc(Expression<Func<T, object>> thenByExpression)
    {
        ThenByDescExpression = thenByExpression;
        return this;
    }

    public Specification<T> Size(int size)
    {
        Limit = size;
        return this;
    } 
}
