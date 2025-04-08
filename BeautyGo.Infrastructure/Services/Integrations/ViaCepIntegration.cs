using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Contracts.Address;
using BeautyGo.Domain.Core.Primitives.Maybies;
using BeautyGo.Domain.Helpers;
using Newtonsoft.Json;

namespace BeautyGo.Infrastructure.Services.Integrations;

internal class ViaCepIntegration : BeautyGoIntegrationService, IViaCepIntegrationService
{
    public ViaCepIntegration(HttpClient client) : base(client) { }

    public async Task<Maybe<AddressIntegrationResponse>> GetAddressByCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return Maybe<AddressIntegrationResponse>.None ;

        var response = await GetAsync($"{CommonHelper.EnsureNumericOnly(cep)}/json", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Maybe<AddressIntegrationResponse>.None; 

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var addressResponse = JsonConvert.DeserializeObject<AddressIntegrationResponse>(responseContent);

        if (addressResponse.HasError)
            return Maybe<AddressIntegrationResponse>.None;
         
        return Maybe<AddressIntegrationResponse>.From(addressResponse);
    }
}
