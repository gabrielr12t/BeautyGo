using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Businesses;
using BeautyGo.Domain.Core.Abstractions;

namespace BeautyGo.Application.Businesses.Queries.GetBusinessesFilter;

public record GetBusinessesFilterPagedQuery(string Name, string Description, double? Latitude = null, 
    double? Longitude = null, double? RadiusKm = null, int PageSize = 10, int PageNumber = 0)
    : IQuery<IPagedList<BusinessFilterModel>>;
