using BeautyGo.Contracts.Businesses;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Businesses;

namespace BeautyGo.Domain.Repositories;

public interface IBusinessRepository : IBaseRepository<Business>
{
    Task<IPagedList<BusinessFilterModel>> FilterAsync(BusinessFilter filter, CancellationToken cancellationToken = default);
}
