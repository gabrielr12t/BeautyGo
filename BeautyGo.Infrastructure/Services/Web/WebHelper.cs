using BeautyGo.Application.Core.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace BeautyGo.Infrastructure.Services.Web;

public class WebHelper : IWebHelper
{
    #region Fields

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory;

    #endregion

    #region Ctor

    public WebHelper(IHttpContextAccessor httpContextAccessor,
        IUrlHelperFactory urlHelperFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _urlHelperFactory = urlHelperFactory;
    }

    #endregion

    #region Utilities

    protected virtual bool IsRequestAvailable()
    {
        if (_httpContextAccessor?.HttpContext == null)
            return false;

        try
        {
            if (_httpContextAccessor.HttpContext.Request == null)
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    protected virtual bool IsIpAddressSet(IPAddress address)
    {
        var rez = address != null && address.ToString() != IPAddress.IPv6Loopback.ToString();

        return rez;
    }

    #endregion

    #region Methods

    public virtual string GetUrlReferrer()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        //URL referrer is null in some case (for example, in IE 8)
        return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
    }

    public virtual Task<string> GetCurrentIpAddressAsync()
    {
        return Task.Run(() =>
        {
            if (!IsRequestAvailable())
                return string.Empty;

            if (_httpContextAccessor.HttpContext.Connection?.RemoteIpAddress is not IPAddress remoteIp)
                return string.Empty;

            if (remoteIp.Equals(IPAddress.IPv6Loopback))
                return IPAddress.Loopback.ToString();

            return remoteIp.MapToIPv4().ToString();
        });
    }

    public virtual string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
    {
        if (!IsRequestAvailable())
            return string.Empty;

        //get store location
        var location = GetLocation(useSsl ?? IsCurrentConnectionSecured());

        //add local path to the URL
        var pageUrl = $"{location.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.Path}";

        //add query string to the URL
        if (includeQueryString)
            pageUrl = $"{pageUrl}{_httpContextAccessor.HttpContext.Request.QueryString}";

        //whether to convert the URL to lower case
        if (lowercaseUrl)
            pageUrl = pageUrl.ToLowerInvariant();

        return pageUrl;
    }

    public virtual bool IsCurrentConnectionSecured()
    {
        if (!IsRequestAvailable())
            return false;

        return _httpContextAccessor.HttpContext.Request.IsHttps;
    }

    public virtual string GetHost(bool useSsl)
    {
        if (!IsRequestAvailable())
            return string.Empty;

        //try to get host from the request HOST header
        var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
        if (StringValues.IsNullOrEmpty(hostHeader))
            return string.Empty;

        //add scheme to the URL
        var host = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

        //ensure that host is ended with slash
        host = $"{host.TrimEnd('/')}/";

        return host;
    }

    public virtual string GetLocation(bool? useSsl = null)
    {
        var location = string.Empty;

        //get store host
        var host = GetHost(useSsl ?? IsCurrentConnectionSecured());
        if (!string.IsNullOrEmpty(host))
        {
            //add application path base if exists
            location = IsRequestAvailable() ? $"{host.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.PathBase}" : host;
        }

        //ensure that URL is ended with slash
        location = $"{location.TrimEnd('/')}/";

        return location;
    }

    public virtual bool IsStaticResource()
    {
        if (!IsRequestAvailable())
            return false;

        string path = _httpContextAccessor.HttpContext.Request.Path;

        //a little workaround. FileExtensionContentTypeProvider contains most of static file extensions. So we can use it
        //source: https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
        //if it can return content type, then it's a static file
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        return contentTypeProvider.TryGetContentType(path, out var _);
    }

    public virtual bool IsRequestBeingRedirected
    {
        get
        {
            var response = _httpContextAccessor.HttpContext.Response;
            //ASP.NET 4 style - return response.IsRequestBeingRedirected;
            int[] redirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found };

            return redirectionStatusCodes.Contains(response.StatusCode);
        }
    }

    public virtual string GetCurrentRequestProtocol()
    {
        return IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
    }

    public virtual bool IsLocalRequest(HttpRequest req)
    {
        var connection = req.HttpContext.Connection;
        if (IsIpAddressSet(connection.RemoteIpAddress))
        {
            //We have a remote address set up
            return IsIpAddressSet(connection.LocalIpAddress)
                //Is local is same as remote, then we are local
                ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                //else we are remote if the remote IP address is not a loopback address
                : IPAddress.IsLoopback(connection.RemoteIpAddress);
        }

        return true;
    }

    public virtual string GetRawUrl(HttpRequest request)
    {
        //first try to get the raw target from request feature
        //note: value has not been UrlDecoded
        var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

        //or compose raw URL manually
        if (string.IsNullOrEmpty(rawUrl))
            rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";

        return rawUrl;
    }

    public virtual bool IsAjaxRequest(HttpRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Headers == null)
            return false;

        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    public virtual string GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
    }

    #endregion
}
