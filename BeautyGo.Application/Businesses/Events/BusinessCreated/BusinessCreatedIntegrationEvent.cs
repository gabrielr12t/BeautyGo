using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Businesses;
using Newtonsoft.Json;

namespace BeautyGo.Application.Businesses.Events.BusinessCreated;

public class BusinessCreatedIntegrationEvent : IIntegrationEvent
{
    internal BusinessCreatedIntegrationEvent(Business businessCreatedDomainEvent) =>
        BusinessId = businessCreatedDomainEvent.Id;

    [JsonConstructor]
    private BusinessCreatedIntegrationEvent(Guid storeId) => BusinessId = storeId;

    public Guid BusinessId { get; set; }
}
