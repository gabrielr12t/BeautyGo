using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace BeautyGo.Application.Core.Behaviors;

internal sealed class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
        where TResponse : class
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehaviour(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            TResponse response = await next();

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }
}
