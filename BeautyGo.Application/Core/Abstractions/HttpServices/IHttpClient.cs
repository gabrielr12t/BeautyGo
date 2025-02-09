using BeautyGo.Contracts.BeautyGoHttp;
using RestSharp.Authenticators;
using System.Net.Mime;

namespace BeautyGo.Application.Core.Abstractions.HttpServices;

public interface IHttpClient
{
    Task<BeautyGoHttpResponse<T>> PostAsync<T>(string url, string endpoint, IAuthenticator authenticator = null,
        object body = null, IEnumerable<QueryParameterModel> queryStrings = null, string contentType = MediaTypeNames.Application.Json,
        bool throwExceptionIfError = false)
        where T : class;

    Task<BeautyGoHttpResponse<T>> GetAsync<T>(string url, string endpoint, IAuthenticator authenticator = null, IEnumerable<QueryParameterModel> queryStrings = null, string contentType = MediaTypeNames.Application.Json,
        bool throwExceptionIfError = false, IEnumerable<BeautyGoHeader> headers = null)
        where T : class;
}
