using Newtonsoft.Json;

namespace BeautyGo.Contracts.OpenStreetMap;

public class AddressCoodinateIntegrationResponse
{
    [JsonProperty("lon")]
    public string Longitude { get; set; }

    [JsonProperty("lat")]
    public string Latitude { get; set; }
}
