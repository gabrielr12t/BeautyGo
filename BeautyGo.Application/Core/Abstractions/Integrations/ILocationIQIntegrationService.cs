using BeautyGo.Contracts.OpenStreetMap;
using BeautyGo.Domain.Core.Primitives.Maybies;

namespace BeautyGo.Application.Core.Abstractions.Integrations;

public interface ILocationIQIntegrationService
{
    Task<Maybe<AddressCoodinateIntegrationResponse>> GetAddressCoordinateAsync(string street, string number, string city, CancellationToken cancellationToken  = default);
}
