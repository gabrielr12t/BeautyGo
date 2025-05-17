using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Authentication.Events;

public class AuthBearerEvents : JwtBearerEvents
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AuthBearerEvents(
        ILogger logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public override async Task MessageReceived(MessageReceivedContext context)
    {
        var cookieName = $"{BeautyGoCookieDefaults.Prefix}{BeautyGoCookieDefaults.UserTokenCookie}";
        if (context.Request.Cookies.ContainsKey(cookieName))
        {
            //var cookie = context.Request.Cookies[cookieName];
            //var token = await EncryptionHelper.DecryptAsync(cookie, BeautyGoAuthenticationDefaults.SaltDefault);

            //context.HttpContext.Request.Headers.Append("Authorization", $"Bearer {token}");

            //context.Token = token;
        }

        await base.MessageReceived(context);
    }

    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        var cancellationToken = context.Request.HttpContext.RequestAborted;

        await _logger.WarningAsync("Authentication Failed", context.Exception, cancellation: cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await base.AuthenticationFailed(context);
    }

    public override Task Forbidden(ForbiddenContext context)
    {
        return base.Forbidden(context);
    }

    public override async Task Challenge(JwtBearerChallengeContext context)
    {
        var cancellationToken = context.HttpContext.RequestAborted;

        await _logger.WarningAsync($"Authentication Challenge: {context.Error}", cancellation: cancellationToken);

        context.HandleResponse();

        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var originalBodyStream = context.Response.Body;

        var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DomainErrors.General.UnauthorizedUser));
        await originalBodyStream.WriteAsync(byteArray, 0, byteArray.Length, cancellationToken);

        context.Response.Body = originalBodyStream;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
