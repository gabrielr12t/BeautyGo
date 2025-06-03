using BeautyGo.Application.Core.Abstractions.Caching;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Settings;
using System.Globalization;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Caching;

public abstract partial class CacheKeyService : ICacheKeyService
{
    #region Fields

    protected readonly AppSettings _appSettings;

    #endregion

    #region Ctor

    protected CacheKeyService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    #endregion

    #region Utilities

    protected virtual string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
    {
        return prefixParameters?.Any() ?? false
            ? string.Format(prefix, prefixParameters.Select(CreateCacheKeyParameters).ToArray())
            : prefix;
    }

    protected virtual string CreateIdsHash(IEnumerable<Guid> ids)
    {
        var identifiers = ids.ToList();

        if (!identifiers.Any())
            return string.Empty;

        var identifiersString = string.Join(", ", identifiers.OrderBy(id => id));
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(identifiersString), HashAlgorithm);
    }

    protected virtual object CreateCacheKeyParameters(object parameter)
    {
        return parameter switch
        {
            null => "null",
            IEnumerable<Guid> ids => CreateIdsHash(ids),
            IEnumerable<BaseEntity> entities => CreateIdsHash(entities.Select(entity => entity.Id)),
            BaseEntity entity => entity.Id,
            decimal param => param.ToString(CultureInfo.InvariantCulture),
            _ => parameter
        };
    }

    #endregion

    #region Methods

    public virtual CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        return cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
    }

    public virtual CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

        key.CacheTime = _appSettings.Get<CacheSettings>().DefaultCacheTime;

        return key;
    }

    #endregion

    #region Properties

    protected string HashAlgorithm => "SHA1";

    #endregion
}
