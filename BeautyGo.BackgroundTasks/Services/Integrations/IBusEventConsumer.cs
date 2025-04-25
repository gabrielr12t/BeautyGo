using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.Services.Integrations;

internal interface IBusEventConsumer
{
    Task ConsumeAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
