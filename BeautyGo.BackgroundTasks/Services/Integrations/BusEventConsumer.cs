using BeautyGo.Application.Core.Abstractions.Messaging;
using MediatR;

namespace BeautyGo.BackgroundTasks.Services.Integrations;

internal sealed class BusEventConsumer : IBusEventConsumer
{
    private readonly IMediator _mediator;

    public BusEventConsumer(IMediator mediator) =>
        _mediator = mediator;

    public async Task ConsumeAsync(IIntegrationEvent integrationEvent) => 
        await _mediator.Publish(integrationEvent);
}
