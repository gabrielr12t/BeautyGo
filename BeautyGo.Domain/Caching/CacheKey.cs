using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Settings;

namespace BeautyGo.Domain.Caching;

public partial class CacheKey
{
    #region Ctor

    public CacheKey(string key)
    {
        Key = key;
    }

    #endregion

    #region Methods

    public virtual CacheKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
    {
        var cacheKey = new CacheKey(Key);

        if (!keyObjects.Any())
            return cacheKey;

        cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

        return cacheKey;
    }

    #endregion

    #region Properties

    public string Key { get; protected set; }

    public int CacheTime { get; set; } = Singleton<AppSettings>.Instance.Get<CacheSettings>().DefaultCacheTime;

    #endregion
}
