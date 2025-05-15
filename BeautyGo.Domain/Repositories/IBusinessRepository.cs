using BeautyGo.Contracts.Businesses;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Domain.Repositories;

public interface IBusinessRepository : IEFBaseRepository<Business>
{
    Task<IPagedList<BusinessFilterModel>> FilterAsync(BusinessFilter filter, CancellationToken cancellationToken = default);
}
