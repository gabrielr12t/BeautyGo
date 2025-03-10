using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Businesses;
using Newtonsoft.Json;

namespace BeautyGo.Application.Businesses.Commands.AccountConfirmed;

public class BusinessAccountConfirmedIntegrationEvent : IIntegrationEvent
{
    internal BusinessAccountConfirmedIntegrationEvent(Business business) => BusinessId = business.Id;

    [JsonConstructor]
    private BusinessAccountConfirmedIntegrationEvent(Guid businessId) => BusinessId = businessId;

    public Guid BusinessId { get; }
}
