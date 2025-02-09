using BeautyGo.Contracts.ReceitaFederal;
using BeautyGo.Domain.Core.Primitives.Maybies;

namespace BeautyGo.Application.Core.Abstractions.Integrations;

public interface IReceitaFederalIntegrationService
{
    bool IsValidCnpjStatus(string status, string situacao);

    Task<Maybe<CnpjIntegrationResponse>> GetCnpjDataAsync(string cnpj, CancellationToken cancellationToken = default);
}
