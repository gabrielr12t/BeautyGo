using Newtonsoft.Json;

namespace BeautyGo.Contracts.Address;

public class AddressIntegrationResponse
{
    [JsonProperty("cep")]
    public string PostalCode { get; set; }

    [JsonProperty("logradouro")]
    public string Street { get; set; }

    [JsonProperty("complemento")]
    public string Complement { get; set; }

    [JsonProperty("unidade")]
    public string Unit { get; set; }

    [JsonProperty("bairro")]
    public string Neighborhood { get; set; }

    [JsonProperty("localidade")]
    public string City { get; set; }

    [JsonProperty("uf")]
    public string StateAbbreviation { get; set; }

    [JsonProperty("estado")]
    public string State { get; set; }

    [JsonProperty("regiao")]
    public string Region { get; set; }

    [JsonProperty("ibge")]
    public string IbgeCode { get; set; }

    [JsonProperty("gia")]
    public string GiaCode { get; set; }

    [JsonProperty("ddd")]
    public string AreaCode { get; set; }

    [JsonProperty("erro")]
    public bool HasError { get; set; }
}
