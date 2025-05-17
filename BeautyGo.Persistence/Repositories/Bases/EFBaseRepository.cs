using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Extensions;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Persistence.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BeautyGo.Persistence.Repositories.Bases;

internal class EFBaseRepository<TEntity> : IEFBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    #region Fields

    protected readonly BeautyGoContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    #endregion

    #region Ctor

    public EFBaseRepository(
       BeautyGoContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    #endregion

    #region Insert

    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);

        entity.AddDomainEvent(new EntityInsertedEvent<TEntity>(entity));
    }

    public virtual async Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities);

        foreach (var entity in entities)
            entity.AddDomainEvent(new EntityInsertedEvent<TEntity>(entity));
    }

    #endregion

    #region Update

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);

        entity.AddDomainEvent(new EntityUpdatedEvent<TEntity>(entity));

        return Task.CompletedTask;
    }

    #endregion

    #region Delete

    public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);

        entity.AddDomainEvent(new EntityDeletedEvent<TEntity>(entity));

        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);

        foreach (var entity in entities)
            entity.AddDomainEvent(new EntityDeletedEvent<TEntity>(entity));

        return Task.CompletedTask;
    }

    public Task DetachAsync(TEntity entity)
    {
        _dbSet.Entry(entity).State = EntityState.Detached;

        return Task.CompletedTask;
    }

    public virtual Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        foreach (TEntity entity in Query())
        {
            _dbSet.Entry(entity).State = EntityState.Deleted;
            entity.AddDomainEvent(new EntityDeletedEvent<TEntity>(entity));
        }

        return Task.CompletedTask;
    }

    #endregion

    #region SQL

    public virtual async Task<int> ExecuteSqlAsync(string sql,
        IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default) =>
        await _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    public virtual async Task<IList<TEntity>> FromSqlAsync(string sql, CancellationToken cancellationToken = default, params IEnumerable<SqlParameter> parameters) =>
        await _context.Set<TEntity>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);

    #endregion

    #region Query Specification

    public virtual async Task<IList<TEntity>> GetByIdAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
    {
        var specification = new EntityByIdsSpecification<TEntity>(ids);

        return await Query().ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return default;

        var query = Query();

        var specification = new EntityByIdSpecification<TEntity>(id);

        return query
            .ApplySpecification(specification)
            .FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);
    }

    public virtual async Task<TEntity> GetFirstOrDefaultAsync(
        Specification<TEntity> specification,
        bool asTracking = false,
        CancellationToken cancellationToken = default)
    {
        return Query(asTracking)
            .ApplySpecification(specification)
            .FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);
    }

    public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Specification<TEntity> specification, Func<TEntity, TResult> select, bool asTracking = false,
        CancellationToken cancellationToken = default)
    {
        return Query(asTracking)
            .ApplySpecification(specification)
            .Where(specification.ToExpression())
            .Select(select)
            .AsQueryable()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistAsync(
        Specification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return Query(false)
            .ApplySpecification(specification)
            .AnyAsync(cancellationToken);
    }

    public virtual async Task<IList<TEntity>> GetAsync(
        Specification<TEntity> specification,
        bool asTracking = true, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking)
            .ApplySpecification(specification)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(
        Specification<TEntity> specification,
        Func<TEntity, TResult> select,
        bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking)
            .ApplySpecification(specification)
            .Select(select)
            .AsQueryable()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IPagedList<TResult>> GetAllPagedAsync<TResult>(
        Specification<TEntity> specification,
        Func<TEntity, TResult> resultSelector = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false,
        CancellationToken cancellationToken = default)
    {
        return await Query(false)
            .ApplySpecification(specification)
            .Select(resultSelector)
            .AsQueryable()
            .ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount, cancellationToken);
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(
        Specification<TEntity> specification,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .ApplySpecification(specification)
            .ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount, cancellationToken);
    }

    #endregion

    #region Utilities

    public IQueryable<TEntity> Query(bool asTracking = false)
        => asTracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();

    #endregion

    public IQueryable<TEntity> QueryNoTracking()
        => _dbSet.AsNoTracking();

    public IQueryable<TEntity> Query()
        => _dbSet.AsTracking();
}
