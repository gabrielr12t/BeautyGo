using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Business;
using Newtonsoft.Json;

namespace BeautyGo.Application.Business.Events.BusinessCreated;

public class BeautyBusinessCreatedIntegrationEvent : IIntegrationEvent
{
    internal BeautyBusinessCreatedIntegrationEvent(BeautyBusiness businessCreatedDomainEvent) =>
        BusinessId = businessCreatedDomainEvent.Id;

    [JsonConstructor]
    private BeautyBusinessCreatedIntegrationEvent(Guid storeId) => BusinessId = storeId;

    public Guid BusinessId { get; }
}
