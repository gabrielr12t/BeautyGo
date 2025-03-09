using Newtonsoft.Json;

namespace BeautyGo.Contracts.OpenStreetMap;

public class AddressCoodinateIntegrationResponse
{
    [JsonProperty("lon")]
    public double Longitude { get; set; }

    [JsonProperty("lat")]
    public double Latitude { get; set; }
}
