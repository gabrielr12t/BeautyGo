using BeautyGo.Contracts.Address;
using BeautyGo.Domain.Entities.Common;

namespace BeautyGo.Domain.Repositories;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<IList<NearbyAddressDTO>> GetAddressWithinRadiusAsync(
        double latitude,
        double longitude,
        double radiousKm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
