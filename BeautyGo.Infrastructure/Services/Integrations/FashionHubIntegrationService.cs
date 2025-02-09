using Newtonsoft.Json;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Integrations;

internal abstract class BeautyGoIntegrationService
{
    protected readonly HttpClient _httpClient;

    public BeautyGoIntegrationService(HttpClient client)
    {
        _httpClient = client;
    }

    protected async Task<HttpResponseMessage> PostAsync(string path, 
        object request, CancellationToken cancellationToken = default)
    {
        var jsonRequest = JsonConvert.SerializeObject(request);

        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        return await _httpClient.PostAsync(path, content, cancellationToken);
    }

    protected async Task<HttpResponseMessage> PutAsync(string path, object request,
        CancellationToken cancellationToken = default)
    {
        var jsonRequest = JsonConvert.SerializeObject(request);

        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        return await _httpClient.PutAsync(path, content, cancellationToken);
    }

    protected async Task<HttpResponseMessage> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(path, cancellationToken);

        return response;    
    }
}
