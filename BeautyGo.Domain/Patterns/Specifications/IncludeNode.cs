using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications;

public class IncludeNode<TEntity> 
{
    public LambdaExpression Navigation { get; }
    public List<IncludeNode<object>> ThenIncludes { get; } = new();

    public IncludeNode(LambdaExpression navigation)
    {
        Navigation = navigation;
    }

    public IncludeNode<TProperty> AddThenInclude<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        var thenInclude = new IncludeNode<TProperty>(expression);
        ThenIncludes.Add((IncludeNode<object>)(object)thenInclude);
        return thenInclude;
    }
}
