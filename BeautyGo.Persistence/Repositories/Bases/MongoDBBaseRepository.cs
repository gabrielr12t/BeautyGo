using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Extensions;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Persistence.Extensions;
using LinqToDB;
using MongoDB.Driver;

namespace BeautyGo.Persistence.Repositories.Bases;

internal class MongoDBBaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    #region Fields

    protected readonly IMongoCollection<TEntity> _collection;

    #endregion

    #region Ctor

    public MongoDBBaseRepository(IMongoDatabase database)
    {
        var collectionName = typeof(TEntity).Name;
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    #endregion

    #region Query

    public IQueryable<TEntity> Query() =>
        _collection.AsQueryable();

    public async Task<IList<TEntity>> GetByIdAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.In(p => p.Id, ids);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(p => p.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    #endregion

    #region Insert

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
    }

    #endregion

    #region Update

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(_ => _.Id, entity.Id);
        return _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    #endregion

    #region Remove

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(_ => _.Id, entity.Id);
        return _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
    }

    public Task RemoveAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var ids = entities.Select(p => p.Id).ToList();
        var filter = Builders<TEntity>.Filter.In(p => p.Id, ids);
        return _collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
    }

    public Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        return _collection.DeleteManyAsync(Builders<TEntity>.Filter.Empty, cancellationToken);
    }

    #endregion

    #region Query Specification

    public async Task<bool> ExistAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = Query().ApplySpecification(specification);
        return await query.AnyAsync(cancellationToken);
    }

    public Task<IPagedList<TEntity>> GetAllPagedAsync(Specification<TEntity> specification, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, CancellationToken cancellationToken = default)
    {
        var query = Query().ApplySpecification(specification);

        return query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount, cancellationToken);
    }

    public async Task<IPagedList<TResult>> GetAllPagedAsync<TResult>(Specification<TEntity> specification, Func<TEntity, TResult> resultSelector = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, CancellationToken cancellationToken = default)
    {
        var query = Query()
            .ApplySpecification(specification)
            .Select(resultSelector)
            .AsQueryable();

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount, cancellationToken);
    }

    public async Task<IList<TEntity>> GetAsync(Specification<TEntity> specification, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query().ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IList<TResult>> GetAsync<TResult>(Specification<TEntity> specification, Func<TEntity, TResult> select, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query().ApplySpecification(specification);
        return await query.Select(select).AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetFirstOrDefaultAsync(Specification<TEntity> specification, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query().ApplySpecification(specification);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult> GetFirstOrDefaultAsync<TResult>(Specification<TEntity> specification, Func<TEntity, TResult> select, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query().ApplySpecification(specification);
        return await query.Select(select).AsQueryable().FirstOrDefaultAsync(cancellationToken);
    }

    #endregion 
}
