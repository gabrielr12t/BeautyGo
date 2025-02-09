using BeautyGo.Application.Core.Abstractions.Messaging;
using MediatR;

namespace BeautyGo.BackgroundTasks.Services.Integrations;

internal sealed class IntegrationEventConsumer : IIntegrationEventConsumer
{
    private readonly IMediator _mediator;

    public IntegrationEventConsumer(IMediator mediator) =>
        _mediator = mediator;

    public async Task ConsumeAsync(IIntegrationEvent integrationEvent) => 
        await _mediator.Publish(integrationEvent);
}
