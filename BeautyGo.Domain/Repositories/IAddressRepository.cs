using BeautyGo.Contracts.Address;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Domain.Repositories;

public interface IAddressRepository : IEFBaseRepository<Address>
{
    Task<IList<NearbyAddressDTO>> GetAddressWithinRadiusAsync(
        double latitude,
        double longitude,
        double radiousKm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
