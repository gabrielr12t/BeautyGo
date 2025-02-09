using Microsoft.AspNetCore.Http;

namespace BeautyGo.Application.Core.Abstractions.Web;

public interface IWebHelper
{
    string GetUrlReferrer();

    Task<string> GetCurrentIpAddressAsync();

    string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);

    bool IsCurrentConnectionSecured();

    string GetStoreHost(bool useSsl);

    string GetStoreLocation(bool? useSsl = null);

    bool IsStaticResource();

    bool IsRequestBeingRedirected { get; }

    string GetCurrentRequestProtocol();

    bool IsLocalRequest(HttpRequest req);

    string GetRawUrl(HttpRequest request);

    bool IsAjaxRequest(HttpRequest request);

    string GetUserAgent();
}
