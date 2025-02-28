using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.Core.Abstractions.OutboxMessages;

public interface IOutboxMessageService
{
    Task PublishAsync(IBusEvent @event, CancellationToken cancellationToken = default);
}
