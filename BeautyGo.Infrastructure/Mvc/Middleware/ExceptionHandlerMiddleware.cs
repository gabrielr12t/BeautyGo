using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Core.Primitives;
using BeautyGo.Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    #region Utilities

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(ex);

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = (int)httpStatusCode;

        await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse(errors));
    }

    private static (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error>) GetHttpStatusCodeAndErrors(Exception exception) =>
        exception switch
        {
            CustomValidationException validationException => (HttpStatusCode.BadRequest, validationException.Errors),
            DomainException domainException => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
            HttpIntegrationException httpIntegrationException => (HttpStatusCode.BadRequest, httpIntegrationException.Errors),
            _ => (HttpStatusCode.InternalServerError, new[] { DomainErrors.General.ServerError })
        };

    #endregion

    public async Task Invoke(HttpContext httpContext, ILogger log, IUnitOfWork unitOfWork, IAuthService authService)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await log.ErrorAsync($"An exception ocurred: {ex.Message}", ex, await authService.GetCurrentUserAsync(httpContext.RequestAborted), httpContext.RequestAborted);

            await HandleExceptionAsync(httpContext, ex);

            await unitOfWork.SaveChangesAsync(httpContext.RequestAborted);
        }
    }
}
