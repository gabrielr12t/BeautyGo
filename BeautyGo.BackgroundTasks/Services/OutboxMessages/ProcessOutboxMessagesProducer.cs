using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BeautyGo.BackgroundTasks.Services.OutboxMessages;

internal class ProcessOutboxMessagesProducer : IProcessOutboxMessagesProducer
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly IEventBus _publisher;
    private readonly IOutboxMessageRepository _outboxRepository;

    #endregion

    #region Ctor

    public ProcessOutboxMessagesProducer(
       IUnitOfWork unitOfWork,
       ILogger logger,
       IEventBus publisher,
       IOutboxMessageRepository outboxRepository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
        _outboxRepository = outboxRepository;
    }

    #endregion

    #region Utilities

    private async Task PublishMessage(
        OutboxMessage message,
        ConcurrentQueue<OutboxUpdate> updateQueue,
        CancellationToken cancellationToken)
    {
        try
        {
            await _logger.InformationAsync($"Publishin outbox message on event bus: {message.Id}");

            var deserializedMessage = JsonConvert
                .DeserializeObject<IIntegrationEvent>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            await _publisher.PublishAsync(deserializedMessage, cancellationToken);

            updateQueue.Enqueue(new OutboxUpdate { Id = message.Id, ProcessedOn = DateTime.Now });
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"Error publishing outbox message on event bus: {message.Id}");

            updateQueue.Enqueue(
                new OutboxUpdate { Id = message.Id, ProcessedOn = DateTime.Now, Error = ex.ToString() });
        }
        finally
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    private struct OutboxUpdate
    {
        public Guid Id { get; init; }
        public DateTime ProcessedOn { get; init; }
        public string? Error { get; init; }
    }

    #endregion

    public async Task ProduceAsync(CancellationToken cancellationToken)
    {
        var unprocessedOutboxMessages = await _outboxRepository.GetRecentUnprocessedOutboxMessages(10);

        var updateQueue = new ConcurrentQueue<OutboxUpdate>();

        var publishTasks = unprocessedOutboxMessages
            .Select(p => PublishMessage(p, updateQueue, cancellationToken))
            .ToList();

        await Task.WhenAll(publishTasks);

        if (!updateQueue.IsEmpty)
        {
            foreach (var updatedMessage in updateQueue)
            {
                var message = await _outboxRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<OutboxMessage>(updatedMessage.Id), true);
                if (message != null)
                {
                    message.ProcessedOn = updatedMessage.ProcessedOn;
                    message.Error = updatedMessage.Error;

                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}
