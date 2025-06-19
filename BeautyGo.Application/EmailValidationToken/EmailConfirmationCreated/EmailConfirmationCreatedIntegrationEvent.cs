using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities;
using Newtonsoft.Json;

namespace BeautyGo.Application.EmailValidationToken.EmailConfirmationCreated;

public class EmailConfirmationCreatedIntegrationEvent : IIntegrationEvent
{
    internal EmailConfirmationCreatedIntegrationEvent(EmailConfirmation BeautyGoEmailToken) =>
        BeautyGoEmailTokenId = BeautyGoEmailToken.Id;

    [JsonConstructor]
    private EmailConfirmationCreatedIntegrationEvent(Guid beautyGoEmailTokenId) => BeautyGoEmailTokenId = beautyGoEmailTokenId;

    public Guid BeautyGoEmailTokenId { get; }
}
