using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Common.Defaults;

public static partial class BeautyGoEntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    public static CacheKey ByIdCacheKey => new($"BeautyGo.{EntityTypeName}.byid.{{0}}");

    public static CacheKey ByIdsCacheKey => new($"BeautyGo.{EntityTypeName}.byids.{{0}}");

    public static CacheKey AllCacheKey => new($"BeautyGo.{EntityTypeName}.all.");

    public static string Prefix => $"BeautyGo.{EntityTypeName}.";

    public static string ByIdPrefix => $"BeautyGo.{EntityTypeName}.byid.";

    public static string ByIdsPrefix => $"BeautyGo.{EntityTypeName}.byids.";

    public static string AllPrefix => $"BeautyGo.{EntityTypeName}.all.";
}
