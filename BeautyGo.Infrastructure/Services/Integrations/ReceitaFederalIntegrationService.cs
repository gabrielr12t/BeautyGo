using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Contracts.ReceitaFederal;
using BeautyGo.Domain.Core.Primitives.Maybies;
using BeautyGo.Domain.Helpers;
using Newtonsoft.Json;

namespace BeautyGo.Infrastructure.Services.Integrations;

internal class ReceitaFederalIntegrationService : BeautyGoIntegrationService, IReceitaFederalIntegrationService
{
    public ReceitaFederalIntegrationService(HttpClient client) : base(client)
    {
    }

    public bool IsValidCnpjStatus(string status, string situacao)
    {
        return string.Equals(status, CnpjIntegrationStatus.Ok, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(situacao, CnpjIntegrationStatus.Ativa, StringComparison.OrdinalIgnoreCase);
    }

    public async Task<Maybe<CnpjIntegrationResponse>> GetCnpjDataAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return Maybe<CnpjIntegrationResponse>.None;

        var response = await GetAsync($"cnpj/{CommonHelper.EnsureNumericOnly(cnpj)}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Maybe<CnpjIntegrationResponse>.None;

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var cnpjResponse = JsonConvert.DeserializeObject<CnpjIntegrationResponse>(responseContent);

        if(string.Equals(cnpjResponse.Status, "ERROR", StringComparison.OrdinalIgnoreCase))
            return Maybe<CnpjIntegrationResponse>.None;

        return Maybe<CnpjIntegrationResponse>.From(cnpjResponse);
    }
}
