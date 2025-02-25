using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Infrastructure;
using LinqToDB;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Extensions;

public static class AsyncIQueryableExtensions
{
    public static Task<bool> AllAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate)
    {
        return AsyncExtensions.AllAsync(source, predicate);
    }

    #region Average

    public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, int>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }
     
    public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, int?>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, long>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, long?>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<float> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, float>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<float?> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, float?>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, double>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, double?>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<decimal> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, decimal>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    public static Task<decimal?> AverageAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, decimal?>> predicate)
    {
        return AsyncExtensions.AverageAsync(source, predicate);
    }

    #endregion

    public static Task<bool> ContainsAsync<TSource>(this IQueryable<TSource> source, TSource item)
    {
        return AsyncExtensions.ContainsAsync(source, item);
    }

    public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate = null)
    {
        return predicate == null ? AsyncExtensions.CountAsync(source) : AsyncExtensions.CountAsync(source, predicate);
    }

    public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate = null)
    {
        return predicate == null ? AsyncExtensions.FirstAsync(source) : AsyncExtensions.FirstAsync(source, predicate);
    }

    public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate = null)
    {
        return predicate == null ? AsyncExtensions.LongCountAsync(source) : AsyncExtensions.LongCountAsync(source, predicate);
    }

    public static Task<TSource> MaxAsync<TSource>(this IQueryable<TSource> source)
    {
        return AsyncExtensions.MaxAsync(source);
    }

    public static Task<TResult> MaxAsync<TSource, TResult>(this IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> predicate)
    {
        return AsyncExtensions.MaxAsync(source, predicate);
    }
 
    public static Task<TSource> MinAsync<TSource>(this IQueryable<TSource> source)
    {
        return AsyncExtensions.MinAsync(source);
    }

    public static Task<TResult> MinAsync<TSource, TResult>(this IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> predicate)
    {
        return AsyncExtensions.MinAsync(source, predicate);
    }

    public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate = null)
    {
        return predicate == null ? AsyncExtensions.SingleAsync(source) : AsyncExtensions.SingleAsync(source, predicate);
    }
    public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate = null)
    {
        return predicate == null ? AsyncExtensions.SingleOrDefaultAsync(source) : AsyncExtensions.SingleOrDefaultAsync(source, predicate);
    }

    #region Sum
 
    public static Task<decimal> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, decimal>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<decimal?> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, decimal?>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, double?>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<float?> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, float?>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, double>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, int>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, int?>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, long>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }
     
    public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, long?>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    public static Task<float> SumAsync<TSource>(this IQueryable<TSource> source,
        Expression<Func<TSource, float>> predicate)
    {
        return AsyncExtensions.SumAsync(source, predicate);
    }

    #endregion

    public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
        this IQueryable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer = null) where TKey : notnull
    {
        return comparer == null
            ? AsyncExtensions.ToDictionaryAsync(source, keySelector, elementSelector)
            : AsyncExtensions.ToDictionaryAsync(source, keySelector, elementSelector, comparer);
    }

    public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQueryable<TSource> source,
        Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer = null) where TKey : notnull
    {
        return comparer == null
            ? AsyncExtensions.ToDictionaryAsync(source, keySelector)
            : AsyncExtensions.ToDictionaryAsync(source, keySelector, comparer);
    }

    //public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source)
    //{
    //    return AsyncExtensions.ToListAsync(source);
    //}

    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
    {
        if (source == null)
            return new PagedList<T>(new List<T>(), pageIndex, pageSize);

        //min allowed page size is 1
        pageSize = Math.Max(pageSize, 1);

        var count = await source.CountAsync();

        var data = new List<T>();

        if (!getOnlyTotalCount)
            data.AddRange(await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());

        return new PagedList<T>(data, pageIndex, pageSize, count);
    }
}
