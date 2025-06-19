namespace BeautyGo.Domain.Caching;

public partial interface ICacheKeyService
{
    CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters);

    CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters);

    CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters);
}