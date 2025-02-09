using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.Services.Integrations;

internal interface IIntegrationEventConsumer
{
    Task ConsumeAsync(IIntegrationEvent integrationEvent);
}
