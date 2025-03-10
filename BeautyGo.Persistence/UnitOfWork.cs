using BeautyGo.Application.Core.Abstractions.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace BeautyGo.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BeautyGoContext _beautyGoContext;

    public UnitOfWork(BeautyGoContext beautyGoContext) => _beautyGoContext = beautyGoContext;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _beautyGoContext.Database.BeginTransactionAsync(cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _beautyGoContext.SaveChangesAsync(cancellationToken);
}
