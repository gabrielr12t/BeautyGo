using BeautyGo.Application.Common.BackgroundServices;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Outbox;
using BeautyGo.Domain.Repositories;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BeautyGo.BackgroundTasks.Services.OutboxMessages;

internal class ProcessOutboxMessagesProducer : IProcessOutboxMessagesProducer
{
    #region Fields

    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly IPublisherBusEvent _publisher;
    private readonly IOutboxMessageRepository _outboxRepository;
    private readonly SemaphoreSlim _semaphore;

    private const int _maxAttemptsFailed = 3;

    #endregion

    #region Ctor

    public ProcessOutboxMessagesProducer(
       IUnitOfWork unitOfWork,
       ILogger logger,
       IPublisherBusEvent publisher,
       IOutboxMessageRepository outboxRepository,
       IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
        _outboxRepository = outboxRepository;
        _semaphore = new(1, 1);
        _mediator = mediator;
    }

    #endregion

    #region Utilities

    private record OutboxUpdate(Guid Id, DateTime ProcessedOn, string Error, Exception Exception, int Attempts);

    private async Task PublishMessageAsync(
        OutboxMessage message,
        ConcurrentQueue<OutboxUpdate> updateQueue,
        CancellationToken cancellationToken)
    {
        try
        {
            await _logger.InformationAsync($"Publishing outbox message on event bus: {message.Id}");

            var deserializedMessage = JsonConvert
                .DeserializeObject<IIntegrationEvent>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            await _publisher.PublishAsync(deserializedMessage, cancellationToken);

            updateQueue.Enqueue(new OutboxUpdate(message.Id, DateTime.Now, null, null, message.Attempts));
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"Error publishing outbox message {message.Id}: {ex.Message}");

            message.Attempts++;

            updateQueue.Enqueue(new OutboxUpdate(message.Id, DateTime.Now, ex.ToString(), ex, message.Attempts));
        }
        finally
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    private async Task ProcessUpdateAsync(OutboxUpdate updatedMessage, CancellationToken cancellationToken)
    {
        var message = await _outboxRepository.GetByIdAsync(updatedMessage.Id, cancellationToken);
        if (message is null)
            return;

        UpdateMessageState(message, updatedMessage);

        if (HasReachedMaxAttempts(message))
        {
            await HandleMaxAttemptsReachedAsync(message, updatedMessage.Exception!, cancellationToken);
        }

        await _outboxRepository.UpdateAsync(message);
    }

    private void UpdateMessageState(OutboxMessage message, OutboxUpdate updatedMessage)
    {
        if (updatedMessage.Error is not null)
        {
            message.Errors.Add(new(updatedMessage.Error, updatedMessage.Exception?.StackTrace));
        }
        else
        {
            message.ProcessedOn = updatedMessage.ProcessedOn;
        }

        message.Attempts = updatedMessage.Attempts;
    }

    private bool HasReachedMaxAttempts(OutboxMessage message)
        => message.Attempts >= _maxAttemptsFailed;

    private async Task HandleMaxAttemptsReachedAsync(OutboxMessage message, Exception exception, CancellationToken cancellationToken)
    {
        message.ProcessedOn = DateTime.Now;

        await _mediator.Publish(new ProcessOuboxMessageFailedEvent(message.Id, exception), cancellationToken);
    }

    #endregion

    public async Task ProduceAsync(CancellationToken cancellationToken)
    {
        var unprocessedOutboxMessages = await _outboxRepository.GetRecentUnprocessedOutboxMessages(5, cancellationToken);

        var updateQueue = new ConcurrentQueue<OutboxUpdate>();

        var publishTasks = unprocessedOutboxMessages
            .Select(outboxMesage => PublishMessageAsync(outboxMesage, updateQueue, cancellationToken))
            .ToList();

        await Task.WhenAll(publishTasks);

        if (!updateQueue.IsEmpty)
        {
            foreach (var updatedMessage in updateQueue)
            {
                await ProcessUpdateAsync(updatedMessage, cancellationToken);
            }
        }
    }
}
