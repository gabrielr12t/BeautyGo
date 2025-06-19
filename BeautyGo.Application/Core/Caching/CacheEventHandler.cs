using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Infrastructure;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Application.Core.Caching;

public abstract class CacheEventHandler<TEntity> :
    IDomainEventHandler<EntityInsertedEvent<TEntity>>,
    IDomainEventHandler<EntityUpdatedEvent<TEntity>>,
    IDomainEventHandler<EntityDeletedEvent<TEntity>>
    where TEntity : BaseEntity
{
    #region Fields

    protected readonly IStaticCacheManager _cacheManager;

    #endregion

    #region Ctor

    protected CacheEventHandler()
    {
        _cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
    }

    #endregion

    #region Utilities

    protected virtual async Task ClearCacheAsync(TEntity entity, EntityEventType entityEventType, CancellationToken cancellationToken = default)
    {
        await RemoveByPrefixAsync(BeautyGoEntityCacheDefaults<TEntity>.ByIdsPrefix);
        await RemoveByPrefixAsync(BeautyGoEntityCacheDefaults<TEntity>.AllPrefix);

        if (entityEventType != EntityEventType.Insert)
            await RemoveAsync(BeautyGoEntityCacheDefaults<TEntity>.ByIdCacheKey, entity);

        await ClearCacheAsync(entity, cancellationToken);
    }

    protected virtual Task ClearCacheAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected virtual async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        await _cacheManager.RemoveByPrefixAsync(prefix, prefixParameters);
    }

    public async Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        await _cacheManager.RemoveAsync(cacheKey, cacheKeyParameters);
    }

    #endregion

    #region Methods

    public async Task Handle(EntityInsertedEvent<TEntity> notification, CancellationToken cancellationToken)
    {
        await ClearCacheAsync(notification.Entity, EntityEventType.Insert, cancellationToken);
    }

    public async Task Handle(EntityUpdatedEvent<TEntity> notification, CancellationToken cancellationToken)
    {
        await ClearCacheAsync(notification.Entity, EntityEventType.Update, cancellationToken);
    }

    public async Task Handle(EntityDeletedEvent<TEntity> notification, CancellationToken cancellationToken)
    {
        await ClearCacheAsync(notification.Entity, EntityEventType.Delete, cancellationToken);
    }

    #endregion

    #region Nested

    protected enum EntityEventType
    {
        Insert,
        Update,
        Delete,
    }

    #endregion
}
