﻿using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Extensions;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;
using BeautyGo.Persistence.Extensions;
using LinqToDB.SqlQuery;
using Microsoft.EntityFrameworkCore;

namespace BeautyGo.Persistence.Repositories;

internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    #region Fields

    private readonly BeautyGoContext _context;
    private readonly DbSet<TEntity> _dbSet;

    #endregion

    #region Ctor

    public BaseRepository(
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

    public virtual async Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);

        foreach (var entity in entities)
            entity.AddDomainEvent(new EntityInsertedEvent<TEntity>(entity));
    }

    #endregion

    #region Update

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);

        entity.AddDomainEvent(new EntityUpdatedEvent<TEntity>(entity));
    }

    #endregion

    #region Delete

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);

        entity.AddDomainEvent(new EntityDeletedEvent<TEntity>(entity));
    }

    public virtual void Remove(IReadOnlyCollection<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);

        foreach (var entity in entities)
            entity.AddDomainEvent(new EntityDeletedEvent<TEntity>(entity));
    }

    public virtual void Truncate()
    {
        foreach (TEntity entity in Query())
        {
            _dbSet.Entry(entity).State = EntityState.Deleted;
            entity.AddDomainEvent(new EntityDeletedEvent<TEntity>(entity));
        }
    }

    #endregion

    #region Raw

    public virtual async Task<int> ExecuteSqlAsync(string sql,
        IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default) =>
        await _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    #endregion

    #region Query Specification

    public virtual async Task<IList<TEntity>> GetByIdAsync(IReadOnlyList<Guid> ids)
    {
        var specification = new EntityByIdsSpecification<TEntity>(ids);

        return await Query().GetQuerySpecification(specification).ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return default;

        var query = Query();

        var specification = new EntityByIdSpecification<TEntity>(id);

        return await query
            .GetQuerySpecification(specification)
            .FirstOrDefaultAsync(specification.ToExpression());
    }

    public virtual async Task<TEntity> GetFirstOrDefaultAsync(
        Specification<TEntity> specification,
        bool asTracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = Query(asTracking);

        return await query
            .GetQuerySpecification(specification)
            .FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);
    }

    public virtual async Task<bool> ExistAsync(
        Specification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await Query(false)
            .GetQuerySpecification(specification)
            .AnyAsync(cancellationToken);
    }

    public virtual async Task<IList<TEntity>> GetAsync(
        Specification<TEntity> specification,
        bool asTracking = true)
    {
        return await Query(asTracking)
            .GetQuerySpecification(specification)
            .ToListAsync();
    }

    public virtual async Task<IPagedList<TResult>> GetAllPagedAsync<TResult>(
        Specification<TEntity> specification,
        Func<TEntity, TResult> resultSelector = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        return await Query()
            .GetQuerySpecification(specification)
            .Select(resultSelector)
            .AsQueryable()
            .ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(
        Specification<TEntity> specification,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        return await Query()
            .GetQuerySpecification(specification)
            .ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    #endregion

    public IQueryable<TEntity> Query(bool asTracking = false)
        => asTracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();
}
