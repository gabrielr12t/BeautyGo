using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Contracts.OpenStreetMap;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Primitives.Maybies;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Settings;
using Newtonsoft.Json;

namespace BeautyGo.Infrastructure.Services.Integrations;

internal class LocationIQIntegrationServiceIntegrationService : BeautyGoIntegrationService, ILocationIQIntegrationService
{
    private readonly LocationIQSettings _locationIQSettings;

    public LocationIQIntegrationServiceIntegrationService(HttpClient client) : base(client)
    {
        _locationIQSettings = Singleton<AppSettings>.Instance.Get<LocationIQSettings>();
    }

    public async Task<Maybe<AddressCoodinateIntegrationResponse>> GetAddressCoordinateAsync(string street, string number, string city,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city))
            return Maybe<AddressCoodinateIntegrationResponse>.None;

        var address = $"{street} {number}, {city}, Brazil";

        var response = await GetAsync($"search?q={Uri.EscapeDataString(address)}&key={_locationIQSettings.Token}&format=json", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Maybe<AddressCoodinateIntegrationResponse>.None;

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var addressResponse = JsonConvert.DeserializeObject<AddressCoodinateIntegrationResponse[]>(responseContent);

        return Maybe<AddressCoodinateIntegrationResponse>.From(addressResponse.FirstOrDefault());
    }
}
