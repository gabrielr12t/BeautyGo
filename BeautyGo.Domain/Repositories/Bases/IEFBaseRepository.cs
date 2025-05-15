using BeautyGo.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace BeautyGo.Domain.Repositories.Bases;

public interface IEFBaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    IQueryable<TEntity> QueryNoTracking();

    Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default);

    Task<IList<TEntity>> FromSqlAsync(string sql, CancellationToken cancellationToken = default, params IEnumerable<SqlParameter> parameters);

    Task DetachAsync(TEntity entity);
}
