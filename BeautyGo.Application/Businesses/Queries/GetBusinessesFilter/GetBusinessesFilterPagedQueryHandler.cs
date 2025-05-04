using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Businesses;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Queries.GetBusinessesFilter;

internal class GetBusinessesFilterPagedQueryHandler
    : IQueryHandler<GetBusinessesFilterPagedQuery, IPagedList<BusinessFilterModel>>
{
    private readonly IBusinessRepository _businessRepository;

    public GetBusinessesFilterPagedQueryHandler(
        IBusinessRepository businessRepository)
    {
        _businessRepository = businessRepository;
    }

    public async Task<IPagedList<BusinessFilterModel>> Handle(GetBusinessesFilterPagedQuery request, CancellationToken cancellationToken)
    {
        var filter = new BusinessFilter(request.Name, request.Description, request.Latitude, request.Longitude,
            request.RadiusKm, request.PageSize, request.PageNumber);

        return await _businessRepository.FilterAsync(filter, cancellationToken);
    }
}
