using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Stores;
using Newtonsoft.Json;

namespace BeautyGo.Application.Stores.Events.StoreCreated;

public class StoreCreatedIntegrationEvent : IIntegrationEvent
{
    internal StoreCreatedIntegrationEvent(Store storeCreatedDomainEvent) =>
        StoreId = storeCreatedDomainEvent.Id;

    [JsonConstructor]
    private StoreCreatedIntegrationEvent(Guid storeId) => StoreId = storeId;

    public Guid StoreId { get; }
}
