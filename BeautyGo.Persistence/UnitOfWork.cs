using BeautyGo.Application.Core.Abstractions.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace BeautyGo.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BeautyGoContext _beautyGoContext;

    public UnitOfWork(BeautyGoContext beautyGoContext) => _beautyGoContext = beautyGoContext;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _beautyGoContext.Database.BeginTransactionAsync(cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _beautyGoContext.SaveChangesAsync(cancellationToken);
}
