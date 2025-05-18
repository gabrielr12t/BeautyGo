using Microsoft.AspNetCore.Http;
using System.Net;

namespace BeautyGo.Domain.Core.Http;

public static class HttpRequestExtensions
{
    public static bool IsPostRequest(this HttpRequest request)
    {
        return request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool IsGetRequest(this HttpRequest request)
    {
        return request.Method.Equals(WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase);
    }
}
