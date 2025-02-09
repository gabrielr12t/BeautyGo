using BeautyGo.Contracts.Address;
using BeautyGo.Domain.Core.Primitives.Maybies;

namespace BeautyGo.Application.Core.Abstractions.Integrations;

public interface IViaCepIntegrationService
{
    Task<Maybe<AddressIntegrationResponse>> GetAddressByCepAsync(string cep, CancellationToken cancellationToken = default);
}
