using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger log, IUnitOfWork unitOfWork)
    {
        //DEVE SER O PRIMEIRO MIDDLEWARE DEPOIS DO AUTHORIZE
        //SALVAR ENDPOINT CHAMADO E O QUE FOI PASSADO NA ROTA E BODY
        await log.InformationAsync("BeautyGo Request received");

        await unitOfWork.SaveChangesAsync();

        await _next(httpContext);

        //SALVAR RESPONSE
    }
}
