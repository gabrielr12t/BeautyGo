using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Contracts.OpenStreetMap;
using BeautyGo.Domain.Core.Primitives.Maybies;
using Newtonsoft.Json;

namespace BeautyGo.Infrastructure.Services.Integrations;

internal class OpenStreetMapIntegrationService : BeautyGoIntegrationService, IOpenStreetMapIntegrationService
{
    public OpenStreetMapIntegrationService(HttpClient client) : base(client)
    {
    }

    public async Task<Maybe<AddressCoodinateIntegrationResponse>> GetAddressCoordinateAsync(string street, string number, string city,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city))
            return Maybe<AddressCoodinateIntegrationResponse>.None;

        var address = $"{street} {number}, {city}, Brasil";

        var response = await GetAsync($"search?q={Uri.EscapeDataString(address)}&format=json&limit=1",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Maybe<AddressCoodinateIntegrationResponse>.None;

        var responseContent = await response.Content.ReadAsStringAsync();

        var addressResponse = JsonConvert.DeserializeObject<AddressCoodinateIntegrationResponse[]>(responseContent);

        return Maybe<AddressCoodinateIntegrationResponse>.From(addressResponse.FirstOrDefault());
    }
}
