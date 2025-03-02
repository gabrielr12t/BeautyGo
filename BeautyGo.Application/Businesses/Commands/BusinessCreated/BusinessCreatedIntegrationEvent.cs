using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Businesses;
using Newtonsoft.Json;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

public class BusinessCreatedIntegrationEvent : IBusEvent
{
    internal BusinessCreatedIntegrationEvent(Business businessCreatedDomainEvent) =>
        BusinessId = businessCreatedDomainEvent.Id;

    [JsonConstructor]
    private BusinessCreatedIntegrationEvent(Guid storeId) => BusinessId = storeId;

    public Guid BusinessId { get; set; }
}
