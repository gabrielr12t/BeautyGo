using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Patterns.Specifications;

namespace BeautyGo.Domain.Repositories.Bases;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> Query(bool asTracking = false);
    
    Task<IList<TEntity>> GetByIdAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default);

    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellation = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default);

    Task TruncateAsync();

    Task<IPagedList<TEntity>> GetAllPagedAsync(
        Specification<TEntity> specification,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false,
        CancellationToken cancellationToken = default);

    Task<IPagedList<TResult>> GetAllPagedAsync<TResult>(
        Specification<TEntity> specification,
        Func<TEntity, TResult> resultSelector = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false,
        CancellationToken cancellationToken = default);

    Task<bool> ExistAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<IList<TEntity>> GetAsync(Specification<TEntity> specification, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IList<TResult>> GetAsync<TResult>(Specification<TEntity> specification, Func<TEntity, TResult> select, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<TEntity> GetFirstOrDefaultAsync(Specification<TEntity> specification, bool asTracking = false,
        CancellationToken cancellationToken = default);

    Task<TResult> GetFirstOrDefaultAsync<TResult>(Specification<TEntity> specification, Func<TEntity, TResult> select, bool asTracking = false,
        CancellationToken cancellationToken = default);
}
