using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Patterns.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeautyGo.Persistence.Extensions;

internal static class SpecificationQueryableExtensions
{
    internal static IQueryable<T> GetQuerySpecification<T>(this IQueryable<T> query, Specification<T> specification)
        where T : BaseEntity
    {
        Expression<Func<T, bool>> criteria = specification.ToExpression();

        if (criteria != null)
            query = query.Where(criteria);

        query = specification.IncludeExpression
            .Aggregate(query,
            (current, include) => current.Include(include));

        query = specification.IncludeStrings
            .Aggregate(query,
            (current, include) => current.Include(include));

        if (specification.OrderByExpression != null)
        {
            var orderedQuery = query.OrderBy(specification.OrderByExpression);

            if (specification.ThenByExpression != null)
                orderedQuery = orderedQuery.ThenBy(specification.ThenByExpression);

            query = orderedQuery;
        }
        else if (specification.OrderByDescExpression != null)
        {
            var orderedQuery = query.OrderByDescending(specification.OrderByDescExpression);

            if (specification.ThenByDescExpression != null)
                orderedQuery = orderedQuery.ThenByDescending(specification.ThenByDescExpression);

            query = orderedQuery;
        }

        if (specification.Limit.HasValue)
            query = query.Take(specification.Limit.Value);

        return query;
    }
}
