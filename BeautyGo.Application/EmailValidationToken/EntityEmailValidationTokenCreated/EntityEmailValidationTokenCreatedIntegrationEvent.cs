using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities;
using Newtonsoft.Json;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

public class EntityEmailValidationTokenCreatedIntegrationEvent : IIntegrationEvent
{
    internal EntityEmailValidationTokenCreatedIntegrationEvent(BeautyGoEmailTokenValidation BeautyGoEmailToken) =>
        BeautyGoEmailTokenId = BeautyGoEmailToken.Id;

    [JsonConstructor]
    private EntityEmailValidationTokenCreatedIntegrationEvent(Guid BeautyGoEmailTokenId) => BeautyGoEmailTokenId = BeautyGoEmailTokenId;

    public Guid BeautyGoEmailTokenId { get; }
}
