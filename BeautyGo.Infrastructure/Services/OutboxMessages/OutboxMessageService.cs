using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Repositories;
using Newtonsoft.Json;

namespace BeautyGo.Infrastructure.Services.OutboxMessages;

internal class OutboxMessageService : IOutboxMessageService
{
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OutboxMessageService(
        IOutboxMessageRepository outboxMessageRepository,
        IUnitOfWork unitOfWork)
    {
        _outboxMessageRepository = outboxMessageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var type = @event.GetType().Name;
        var content = JsonConvert.SerializeObject(
            @event,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

        var outbox = OutboxMessage.Create(type, content);

        await _outboxMessageRepository.InsertAsync(outbox, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
