using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;

    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        var request = context.Request;

        if (context.RequestServices.GetService<IQuery<object>>() != null)
        {
            await _next(context);
            return;
        }

        await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync(context.RequestAborted);

        try
        {
            await _next(context);

            await transaction.CommitAsync(context.RequestAborted);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(context.RequestAborted);
            throw;
        }
    }
}

