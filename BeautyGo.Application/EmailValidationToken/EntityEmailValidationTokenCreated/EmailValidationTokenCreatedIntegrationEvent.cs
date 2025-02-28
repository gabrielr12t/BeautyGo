using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities;
using Newtonsoft.Json;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

public class EmailValidationTokenCreatedIntegrationEvent : IBusEvent
{
    internal EmailValidationTokenCreatedIntegrationEvent(EmailTokenValidation BeautyGoEmailToken) =>
        BeautyGoEmailTokenId = BeautyGoEmailToken.Id;

    [JsonConstructor]
    private EmailValidationTokenCreatedIntegrationEvent(Guid beautyGoEmailTokenId) => BeautyGoEmailTokenId = beautyGoEmailTokenId;

    public Guid BeautyGoEmailTokenId { get; }
}
