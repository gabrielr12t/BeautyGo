using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.Core.Abstractions.OutboxMessages;

public interface IOutboxMessageService
{
    Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);
}
